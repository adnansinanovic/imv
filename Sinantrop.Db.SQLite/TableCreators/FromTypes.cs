using System;
using System.Collections.Generic;
using SQLite;

namespace Sinantrop.Db.SQLite.TableCreators
{
    public class FromTypes : ITableCreator
    {
        private readonly List<Type> _types;

        public FromTypes(List<Type> types)
        {
            _types = types;
        }

        public FromTypes(Type type)
        {
            _types = new List<Type> { type };
        }

        public FromTypes(params Type[] types)
        {
            _types = new List<Type>();
            if (types != null)
            {
                _types.AddRange(types);
            }
        }

        public void Create(SQLiteConnection connection)
        {
            _types.ForEach(type => connection.CreateTable(type));
        }

    }
}
