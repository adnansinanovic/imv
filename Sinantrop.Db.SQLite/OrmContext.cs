using System;
using System.IO;
using Sinantrop.Db.SQLite;
using Sinantrop.Helper;
using SQLite;


namespace Sinantrop.DB.SQLite
{
    public class OrmContext
    {
        private const string DB_EXTENSION = "db3";
        private OrmContextSettings _contextSettings;
        private FileInfo _dbFile;
        public string ContextName => _contextSettings.ContextName;

        internal SQLiteConnection GetConnection()
        {
            var con = new SQLiteConnection(_dbFile.FullName, _contextSettings.OpenFlags);                                  
             
            return con;
                        
            //return new SQLiteAsyncConnection(_dbFile.FullName, _contextSettings.OpenFlags, true);
        }

        internal SQLiteAsyncConnection GetAsyncConnection()
        {
            //var con = new SQLiteConnection(_dbFile.FullName, _contextSettings.OpenFlags);

            //return con;

            return new SQLiteAsyncConnection(_dbFile.FullName, _contextSettings.OpenFlags, true);
        }

        public void Initialize(OrmContextSettings contextSettings)
        {
            _contextSettings = contextSettings;

            string dbName = contextSettings.DbName;
            if (string.IsNullOrEmpty(dbName))
            {
                dbName = AppDomain.CurrentDomain.FriendlyName;
                _dbFile = new FileInfo($"{dbName}.{DB_EXTENSION}");
            }
            else
                _dbFile = new FileInfo(dbName);


            if (contextSettings.OpenFlags.IsSet(SQLiteOpenFlags.Create))
            {
                using (var connection = GetConnection())
                {
                    foreach (ITableCreator tableCreator in _contextSettings.TableCreators)
                    {
                        tableCreator.Create(connection);
                    }
                }                                
            }
        }

      
    }
}
