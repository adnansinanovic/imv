using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Sinantrop.IMV.Db;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.SyncLogic
{
    internal class SyncDa
    {
        private readonly DataHelper _dataHelper = new DataHelper();
        public void SetDbPath(string path)
        {
            _dataHelper.DbPath = path;
        }

        public List<Conversation> GetConversations()
        {
            using (DbCommand cmd = _dataHelper.GetSqlStringCommand(" SELECT * FROM Conversation ORDER BY Title; "))
            {
                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, cmd);
                List<Conversation> list = new List<Conversation>();
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    Conversation conversation = new Conversation();
                    conversation.Id = Convert.ToInt32(tbl.Rows[index]["id"]);
                    conversation.Title = tbl.Rows[index]["Title"].ToString();
                    conversation.MessengerType = (MessengerType) Convert.ToInt32(tbl.Rows[index]["MessengerType"]);
                    conversation.Username = tbl.Rows[index]["Username"].ToString();
                    list.Add(conversation);
                }
                return list;
            }
        }

        public IEnumerable<Message> GetMessages(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            using (DbCommand cmd = _dataHelper.GetSqlStringCommand(" SELECT * FROM Message WHERE ConversationId = :cid AND [Timestamp] >= :dtf AND [TimeStamp] <= :dtt; "))
            {

                _dataHelper.AddInParameter(cmd, "cid", DbType.Int32, conversationId);
                _dataHelper.AddInParameter(cmd, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(cmd, "dtt", DbType.Int32, tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, cmd);
                List<Message> list = new List<Message>();
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    Message item = new Message();
                    item.Id = Convert.ToInt32(tbl.Rows[index]["id"]);
                    item.Username = tbl.Rows[index]["Username"].ToString();
                    item.Identity= tbl.Rows[index]["Identity"].ToString();
                    item.Author = tbl.Rows[index]["Author"].ToString();
                    item.Content = tbl.Rows[index]["Content"].ToString();
                    item.ConversationId = tbl.Rows[index]["ConversationId"].ToString();
                    item.MessengerType = (MessengerType)Convert.ToInt32(tbl.Rows[index]["MessengerType"]);
                    item.Timestamp = Convert.ToInt64(tbl.Rows[index]["timestamp"]);                    

                    list.Add(item);
                }
                return list;
            }
        }

        internal IEnumerable<Message> GetMessages(int idFrom, int maxRows)
        {
            
            using (DbCommand cmd = _dataHelper.GetSqlStringCommand(" SELECT * FROM Message WHERE Id >= :mid LIMIT :maxRows; "))
            {

                _dataHelper.AddInParameter(cmd, "idf", DbType.Int32, idFrom);
                _dataHelper.AddInParameter(cmd, "maxRows", DbType.Int32, maxRows);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, cmd);
                List<Message> list = new List<Message>();
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    Message item = new Message();
                    item.Id = Convert.ToInt32(tbl.Rows[index]["id"]);
                    item.Username = tbl.Rows[index]["Username"].ToString();
                    item.Identity = tbl.Rows[index]["Identity"].ToString();
                    item.Author = tbl.Rows[index]["Author"].ToString();
                    item.Content = tbl.Rows[index]["Content"].ToString();
                    item.ConversationId = tbl.Rows[index]["ConversationId"].ToString();
                    item.MessengerType = (MessengerType)Convert.ToInt32(tbl.Rows[index]["MessengerType"]);
                    item.Timestamp = Convert.ToInt64(tbl.Rows[index]["timestamp"]);

                    list.Add(item);
                }
                return list;
            }
        }

        public IEnumerable<ConversationStatistics> GetStatistics(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);
            List<ConversationStatistics> list = new List<ConversationStatistics>();

            using (DbCommand cmd = _dataHelper.GetSqlStringCommand(" SELECT ConversationId, Author, Count (Content) AS total_messages FROM Message WHERE ConversationId = :cid AND [timestamp]  >= :dtf AND [timestamp] <= :dtt GROUP BY ConversationId, Author ORDER BY total_messages DESC; "))
            {

                _dataHelper.AddInParameter(cmd, "cid", DbType.Int32, conversationId);
                _dataHelper.AddInParameter(cmd, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(cmd, "dtt", DbType.Int32, tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, cmd);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    ConversationStatistics item = new ConversationStatistics();
                  
                    item.ConversationId= (ulong)Convert.ToInt32(tbl.Rows[index]["ConversationId"]);
                    item.Identity= tbl.Rows[index]["author"].ToString();
                    item.TotalMessages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }             
            }

            return list;
        }

        public IEnumerable<ConversationStatistics> GetUrlStatistics(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);
            List<ConversationStatistics> list = new List<ConversationStatistics>();

            using (DbCommand cmd = _dataHelper.GetSqlStringCommand($" SELECT ConversationId, Author, Count (Content) AS total_messages FROM Message WHERE ConversationId = :cid AND [timestamp]  >= :dtf AND [timestamp] <= :dtt AND Content REGEXP '{Constants.RegexUrl}'  GROUP BY ConversationId, Author ORDER BY total_messages DESC; "))
            {

                _dataHelper.AddInParameter(cmd, "cid", DbType.Int32, conversationId);
                _dataHelper.AddInParameter(cmd, "dtf", DbType.Int32, tsFrom);
                _dataHelper.AddInParameter(cmd, "dtt", DbType.Int32, tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, cmd);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    ConversationStatistics item = new ConversationStatistics();

                    item.ConversationId = (ulong)Convert.ToInt32(tbl.Rows[index]["ConversationId"]);
                    item.Identity = tbl.Rows[index]["author"].ToString();
                    item.TotalMessages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
