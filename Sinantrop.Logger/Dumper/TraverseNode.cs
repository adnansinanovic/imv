using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sinantrop.Logger.Dumper
{
    internal class TraverseNode
    {
        public string Name { get; set; }
        public string ItemType { get; set; }
        public object Parent { get; set; }

        public object Item { get; set; }
        public int Level { get; set; }

        public TraverseNode(string name, string itemType, object item, object parent, int level)
        {
            Name = name;
            ItemType = itemType;
            Item = item;
            Parent = parent;
            Level = level;
        }

        internal IEnumerable<TraverseNode> GetChildren(DumperConfiguration configuration)
        {
            List<TraverseNode> children = new List<TraverseNode>();

            if (Item == null || Item is ValueType || Item is string)
                return children;

            if (Item is IEnumerable)
                return children;

            List<MemberInfo> members = Item.GetType().GetMembers(configuration.BindingFlags).ToList();
            foreach (MemberInfo member in members)
            {
                Type type = member.DeclaringType;
                if (type != null && type.FullName.Equals($"System.{type.Name}", StringComparison.Ordinal))
                    continue;

                if (type != null && type.FullName.Equals($"System.Reflection.{type.Name}", StringComparison.Ordinal))
                    continue;

                if (IgnoreCompilerGeneratedMember(member, configuration))
                    continue;

                ClassMember cm = new ClassMember(member);
                if (cm.IsValid())
                {
                    object value = cm.GetValue(Item);
                    children.Add(new TraverseNode(cm.GetName(), cm.GetClassMemberType().FullName, value, this, Level + 1));
                }
            }

            return children.OrderBy(x => x.Name);
        }

        private bool IgnoreCompilerGeneratedMember(MemberInfo member, DumperConfiguration configuration)
        {
            if (configuration.WriteCompilerGeneratedTypes)
                return false;

            var result = member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true);

            return result.Length != 0;
        }



        internal NameContainer GetNames()
        {
            return new NameContainer(Name, $"{ItemType}.{Name}");
        }

        private class ClassMember
        {
            private readonly FieldInfo _fieldInfo;
            private readonly PropertyInfo _propertyInfo;
            private readonly MemberInfo _memberInfo;

            public ClassMember(MemberInfo memberInfo)
            {
                _fieldInfo = memberInfo as FieldInfo;
                _propertyInfo = memberInfo as PropertyInfo;
                _memberInfo = memberInfo;
            }

            internal bool IsValid()
            {
                return _fieldInfo != null || _propertyInfo != null;
            }      

            internal object GetValue(object obj)
            {
                object value = null;
                if (_fieldInfo != null)
                {
                    value = _fieldInfo.GetValue(obj);
                }
                else if (_propertyInfo != null)
                {
                    var getMethod = _propertyInfo.GetGetMethod(false);
                    if (getMethod != null)
                        value = _propertyInfo.GetValue(obj, null);
                }

                return value;
            }

            internal string GetName()
            {
                return _memberInfo.Name;
            }
         

            public Type GetClassMemberType()
            {
                return _fieldInfo != null ? _fieldInfo.FieldType : _propertyInfo.PropertyType;
            }
        }
    }
}
