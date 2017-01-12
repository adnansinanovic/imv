using System;
using System.Collections.Generic;
using SQLite;
using Sinantrop.Db.SQLite;
using Sinantrop.Db.SQLite.TableCreators;

namespace Sinantrop.DB.SQLite
{
    public class OrmContextSettings
    {
        public OrmContextSettings()
        {
            ContextName = Guid.NewGuid().ToString();
            OpenFlags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;            
            TableCreators = new List<ITableCreator>();
        }

        public List<ITableCreator> TableCreators { get; set; }

        public string DbName { get; set; }
        public string ContextName { get; set; }        
        public SQLiteOpenFlags OpenFlags { get; set; }
    }
}
