using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Sinantrop.IMV.Db;
using Sinantrop.IMV.SkypeLogic.Model;

namespace Sinantrop.IMV.SkypeLogic
{
    public class SkypeDa
    {
        private readonly DataHelper _dataHelper = new DataHelper();
        public void SetDbPath(string dbpath)
        {
            _dataHelper.DbPath = dbpath;
        }

        public List<SkypeConversations> GetConversations()
        {            
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand("SELECT * FROM Conversations where displayname is not null or given_displayname is not null"))
            {
                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);
                List<SkypeConversations> list = new List<SkypeConversations>();
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    SkypeConversations conversations = new SkypeConversations();
                    conversations.Id = (long)Convert.ToInt32(tbl.Rows[index]["id"]);
                    conversations.displayname = tbl.Rows[index]["displayname"].ToString();
                    conversations.given_displayname = tbl.Rows[index]["given_displayname"].ToString();
                    list.Add(conversations);
                }
                return list;
            }
        }

        public List<SkypeMessage> GetMessages(long conversationiD, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<SkypeMessage> list = new List<SkypeMessage>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(" SELECT convo_id, [timestamp], id, author, from_dispname, body_xml FROM Messages WHERE convo_id = :convo_id and [timestamp] >= :dtf and [timestamp] <= :dtt and Author <> '' and identities is null and author <> 'sys'"))
            {


                _dataHelper.AddInParameter(sqlStringCommand, "convo_id", DbType.Int32, (object)conversationiD);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int32, tsTo);


                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    SkypeMessage message = new SkypeMessage();

                    message.Id = Convert.ToInt64(tbl.Rows[index]["id"]);
                    message.convo_id = (long)Convert.ToInt32(tbl.Rows[index]["convo_id"]);
                    message.author = tbl.Rows[index]["author"].ToString();
                    message.from_dispname = tbl.Rows[index]["from_dispname"].ToString();
                    message.timestamp = Convert.ToInt64(tbl.Rows[index]["timestamp"]);
                    message.body_xml = tbl.Rows[index]["body_xml"].ToString();
                    list.Add(message);
                }
            }
            return list;
        }

        internal List<SkypeMessage> GetMessages(int idFrom, int maxRows)
        {
            List<SkypeMessage> list = new List<SkypeMessage>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(" SELECT convo_id, [timestamp], id, author, from_dispname, body_xml FROM Messages WHERE  id >= :idf and Author <> '' and identities is null and author <> 'sys' LIMIT :maxRows"))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "idf", DbType.Int32, idFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "maxRows", DbType.Int32, maxRows);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    SkypeMessage message = new SkypeMessage();

                    message.Id = Convert.ToInt64(tbl.Rows[index]["id"]);
                    message.convo_id = (long)Convert.ToInt32(tbl.Rows[index]["convo_id"]);
                    message.author = tbl.Rows[index]["author"].ToString();
                    message.from_dispname = tbl.Rows[index]["from_dispname"].ToString();
                    message.timestamp = Convert.ToInt64(tbl.Rows[index]["timestamp"]);
                    message.body_xml = tbl.Rows[index]["body_xml"].ToString();
                    list.Add(message);
                }
            }
            return list;
        }

        public List<SkypeConversationStatistics> GetStatistics(long conversationiD, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<SkypeConversationStatistics> list = new List<SkypeConversationStatistics>();

            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(" SELECT M.convo_id, CASE WHEN C.Fullname IS NULL THEN M.Author ELSE C.fullname END AS Author, count(M.author) AS total_messages FROM Messages M INNER JOIN Contacts C ON M.Author = C.Skypename WHERE M.convo_id = :convo_id and M.[timestamp] >= :dtf and M.[timestamp] <= :dtt AND M.Author <> '' AND M.identities IS NULL AND M.author <> 'sys' GROUP BY M.convo_id, Author ORDER BY total_messages DESC; "))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "convo_id", DbType.Int32, (object)conversationiD);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int32, tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    SkypeConversationStatistics item = new SkypeConversationStatistics();
                    item.Id = index;
                    item.convo_id = Convert.ToInt32(tbl.Rows[index]["convo_id"]);
                    item.author = tbl.Rows[index]["author"].ToString();
                    item.total_messages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }
            }

            return list;
        }

        public List<SkypeConversationStatistics> GetUrlStatistics(long conversationiD, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<SkypeConversationStatistics> list = new List<SkypeConversationStatistics>();

            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(
                $" SELECT M.convo_id, CASE WHEN C.Fullname IS NULL THEN M.Author ELSE C.fullname END AS Author, count(M.author) AS total_messages FROM Messages M INNER JOIN Contacts C ON M.Author = C.Skypename WHERE M.convo_id = :convo_id and M.[timestamp] >= :dtf and M.[timestamp] <= :dtt AND M.Author <> '' AND M.identities IS NULL AND M.author <> 'sys' AND M.body_xml REGEXP '{Constants.RegexUrl}' GROUP BY M.convo_id, Author ORDER BY total_messages DESC; "))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "convo_id", DbType.Int32, (object)conversationiD);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int32, tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    SkypeConversationStatistics item = new SkypeConversationStatistics();
                    item.Id = index;
                    item.convo_id = (long)Convert.ToInt32(tbl.Rows[index]["convo_id"]);
                    item.author = tbl.Rows[index]["author"].ToString();
                    item.total_messages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }
            }

            return list;
        }

        public int GetMinMessageId()
        {
            int minId = -1;
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(" select min(id) from Messages; "))
            {
                object result = _dataHelper.ExecuteScalar(sqlStringCommand);

                if (result != null)
                    minId = Convert.ToInt32(result);
            }

            return minId;
        }
    }
}
