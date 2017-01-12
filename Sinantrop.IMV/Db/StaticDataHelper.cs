using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;

namespace Sinantrop.IMV.Db
{
    public class StaticDataHelper
    {
        private static DbProviderFactory db;
        

        public static DbProviderFactory Database
        {
            get
            {
                return db;
            }
        }

        public static string DbPath { get; set; }

        static StaticDataHelper()
        {

            db = (DbProviderFactory)SQLiteFactory.Instance;
        }        

        private static string GetConnectionString()
        {            
            string cn = string.Format("Data Source={0}", (object)DbPath);

            return cn;
        }

        private static DbConnection GetConnection()
        {
            SQLiteConnection sqLiteConnection = (SQLiteConnection)Database.CreateConnection();

            sqLiteConnection.ConnectionString = GetConnectionString();
            sqLiteConnection.Open();

            ExecuteSqlCommand("PRAGMA synchronous = FULL", (DbConnection)sqLiteConnection, (DbTransaction)null);
            return (DbConnection)sqLiteConnection;
        }

        public static DbCommand GetSqlStringCommand(string sql)
        {
            DbCommand command = Database.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 15;
            return command;
        }

        public static DbParameter AddInParameter(DbCommand cmd, string paramName, DbType paramType, object value)
        {
            DbParameter parameter = Database.CreateParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = paramName;
            parameter.DbType = paramType;
            parameter.Value = value;
            cmd.Parameters.Add((object)parameter);
            return parameter;
        }

        public static void FillTable(DataTable tbl, DbCommand cmd)
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

        public static void FillTable(DataTable tbl, DbCommand cmd, int maxRows)
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

        public static void FillTable(DataSet ds, DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                DbDataAdapter dataAdapter = Database.CreateDataAdapter();
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(ds);
            }
        }

        public static object ExecuteScalar(DbCommand cmd)
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

        public static int ExecuteNonQuery(DbCommand cmd)
        {
            using (DbConnection connection = GetConnection())
            {
                cmd.Connection = connection;
                return cmd.ExecuteNonQuery();
            }
        }

        public static void SetParameterValue(DbCommand cmd, string paramName, object paramValue)
        {
            cmd.Parameters[paramName].Value = paramValue;
        }

        public static void SetParameterInt32Value(DbCommand cmd, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)Convert.ToInt32(paramValue);
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public static void SetParameterDecimalValue(DbCommand cmd, string paramName, string paramValue)
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

        public static void SetParameterDateTimeValue(DbCommand cmd, string paramName, string paramValue, string format, IFormatProvider formatProvider)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)DateTime.ParseExact(paramValue, format, formatProvider);
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public static void SetParameterStringValue(DbCommand cmd, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
                cmd.Parameters[paramName].Value = (object)paramValue;
            else
                cmd.Parameters[paramName].Value = (object)DBNull.Value;
        }

        public static DbDataReader ExecuteReader(DbCommand cmd)
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

        public static DbDataReader ExecuteReader(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteReader();
        }

        public static void FillTable(DataTable tbl, DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            using (DbDataAdapter dataAdapter = Database.CreateDataAdapter())
            {
                dataAdapter.SelectCommand = cmd;
                dataAdapter.Fill(tbl);
            }
        }

        public static int ExecuteNonQuery(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteNonQuery();
        }

        public static object ExecuteScalar(DbCommand cmd, DbConnection cn, DbTransaction trans)
        {
            cmd.Connection = cn;
            cmd.Transaction = trans;
            return cmd.ExecuteScalar();
        }

        public static int ExecuteSqlCommand(string sql, DbConnection cn, DbTransaction trans, params object[] parameters)
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

        public static int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            using (DbConnection connection = GetConnection())
                return ExecuteSqlCommand(sql, connection, (DbTransaction)null, parameters);
        }
    }
}
