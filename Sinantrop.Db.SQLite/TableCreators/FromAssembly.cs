using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoreLinq;
using Sinantrop.DB.SQLite;
using SQLite;

namespace Sinantrop.Db.SQLite.TableCreators
{
    public class FromAssembly : ITableCreator
    {
        private readonly List<Assembly> _assemblies;
        public FromAssembly(List<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public FromAssembly(Assembly assembly)
        {
            _assemblies = new List<Assembly> { assembly };
        }
        
        public void Create(SQLiteConnection connection)
        {
            foreach (var assembly in _assemblies)
            {
                var types = assembly.GetTypes();
                types.Where(x => x.IsSubclassOf(typeof(OrmEntity)))
                    .ForEach(z => connection.CreateTable(z));
            }
        }
    }
}
