using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;

namespace Sinantrop.IMV.Db
{
    public class DataHelper
    {
        private DbProviderFactory db;
        

        public DbProviderFactory Database
        {
            get
            {
                return db;
            }
        }

        public  string DbPath { get; set; }

         public DataHelper()
        {

            db = new SQLiteFactory();            
            //db = (DbProviderFactory)SQLiteFactory.Instance;
        }        

        private  string GetConnectionString()
        {            
            string cn = string.Format("Data Source={0}", (object)DbPath);

            return cn;
        }

        private  DbConnection GetConnection()
        {
            SQLiteConnection sqLiteConnection = (SQLiteConnection)Database.CreateConnection();

            sqLiteConnection.ConnectionString = GetConnectionString();
            sqLiteConnection.Open();

            ExecuteSqlCommand("PRAGMA synchronous = FULL", (DbConnection)sqLiteConnection, (DbTransaction)null);
            return (DbConnection)sqLiteConnection;
        }

        public  DbCommand GetSqlStringCommand(string sql)
        {
            DbCommand command = Database.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 15;
            return command;
        }

        public  DbParameter AddInParameter(DbCommand cmd, string paramName, DbType paramType, object value)
        {
            DbParameter parameter = Database.CreateParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = paramName;
            parameter.DbType = paramType;
            parameter.Value = value;
            cmd.Parameters.Add((object)parameter);
            return parameter;
        }

        public  void FillTable(DataTable tbl, DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                using (DbDataAdapter dataAdapter = Database.CreateDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(tbl);
                }
            }
        }    

        public  void FillTable(DataTable tbl, DbCommand cmd, int maxRows)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                using (DbDataAdapter dataAdapter = Database.CreateDataAdapter())
                {
                    dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(0, maxRows, new DataTable[1]{tbl});
                }
            }
        }

        public  void FillTable(DataSet ds, DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                DbDataAdapter dataAdapter = Database.CreateDataAdapter();
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(ds);
            }
        }

        public  object ExecuteScalar(DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                object obj = cmd.ExecuteScalar();
                if (obj == DBNull.Value)
                    obj = (object)null;
                return obj;
            }
        }

        public  int ExecuteNonQuery(DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                return cmd.ExecuteNonQuery();
            }
        }

        public  void SetParameterValue(DbCommand cmd, string paramName, object paramValue)
        {
            cmd.Parameters[paramName].Value = paramValue;
        }

        public  void SetParameterInt32Value(DbCommand cmd, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)Convert.ToInt32(paramValue);
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public  void SetParameterDecimalValue(DbCommand cmd, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-us");
                Decimal num = Decimal.Parse(paramValue, (IFormatProvider)cultureInfo.NumberFormat);
                cmd.Parameters[paramName].Value = (object)num;
            }
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public  void SetParameterDateTimeValue(DbCommand cmd, string paramName, string paramValue, string format, IFormatProvider formatProvider)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)DateTime.ParseExact(paramValue, format, formatProvider);
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public  void SetParameterStringValue(DbCommand cmd, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)paramValue;
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public  DbDataReader ExecuteReader(DbCommand cmd)
        {
            DbConnection connection = GetConnection();
            try
            {
                cmd.Connection = connection;
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        public  DbDataReader ExecuteReader(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteReader();
        }

        public  void FillTable(DataTable tbl, DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            using (DbDataAdapter dataAdapter = Database.CreateDataAdapter())
            {
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(tbl);
            }
        }

        public  int ExecuteNonQuery(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteNonQuery();
        }

        public  object ExecuteScalar(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteScalar();
        }

        public  int ExecuteSqlCommand(string sql, DbConnection cn, DbTransaction trans, params object[] parameters)
        {
            using (DbCommand sqlStringCommand = GetSqlStringCommand(sql))
            {
                if (parameters != null)
                {
                    int index = 0;
                    while (index < parameters.Length / 2)
                    {
                        DbParameter parameter = Database.CreateParameter();
                        parameter.Direction = ParameterDirection.Input;
                        parameter.ParameterName = "Param" + (object)index;
                        parameter.DbType = (DbType)parameters[index + 1];
                        parameter.Value = parameters[index];
                        sqlStringCommand.Parameters.Add((object)parameter);
                        index += 2;
                    }
                }
                sqlStringCommand.Connection = cn;
                sqlStringCommand.Transaction = trans;
                return sqlStringCommand.ExecuteNonQuery();
            }
        }

        public  int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            using (DbConnection connection = GetConnection())
                return ExecuteSqlCommand(sql, connection, (DbTransaction)null, parameters);
        }
    }
}
