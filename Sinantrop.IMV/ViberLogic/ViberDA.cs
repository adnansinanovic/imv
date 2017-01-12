using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Sinantrop.IMV.Db;
using Sinantrop.IMV.ViberLogic.Model;

namespace Sinantrop.IMV.ViberLogic
{
    public class ViberDA
    {
        private DataHelper _dataHelper;
        public ViberDA()
        {
            _dataHelper = new DataHelper();
        }

        public void SetDbPath(string dbpath)
        {            
            _dataHelper.DbPath = dbpath;
        }

        public List<ViberConversation> GetConversations()
        {
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(ConversationQuery))
            {
                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);
                List<ViberConversation> list = new List<ViberConversation>();
                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    ViberConversation conversations = new ViberConversation();
                    conversations.ChatId = Convert.ToInt32(tbl.Rows[index]["ChatId"]);
                    conversations.Group = tbl.Rows[index]["Group"].ToString();
                    list.Add(conversations);
                }
                return list;
            }
        }

        internal List<ViberConversationStatistics> GetStatistics(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<ViberConversationStatistics> list = new List<ViberConversationStatistics>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(StatisticsQuery))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "cid", DbType.Int32, (object)conversationId);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int64, (object)tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int64, (object)tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    ViberConversationStatistics item = new ViberConversationStatistics();
                    item.Id = index;
                    item.chatId = (long)Convert.ToInt32(tbl.Rows[index]["chatid"]);
                    item.author = tbl.Rows[index]["author"].ToString();
                    item.total_messages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }
            }

            return list;
        }       

        internal List<ViberConversationStatistics> GetUrlStatistics(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<ViberConversationStatistics> list = new List<ViberConversationStatistics>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(StatisticsUrlQuery))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "cid", DbType.Int32, (object)conversationId);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int64, (object)tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int64, (object)tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    ViberConversationStatistics item = new ViberConversationStatistics();
                    item.Id = index;
                    item.chatId = (long)Convert.ToInt32(tbl.Rows[index]["chatid"]);
                    item.author = tbl.Rows[index]["author"].ToString();
                    item.total_messages = (ulong)Convert.ToInt64(tbl.Rows[index]["total_messages"]);
                    list.Add(item);
                }
            }

            return list;
        }

        internal List<ViberMessage> GetMessages(long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            long tsFrom = (long)Helper.DateTimeToUnixTimestamp(dtFrom);
            long tsTo = (long)Helper.DateTimeToUnixTimestamp(dtTo);

            List<ViberMessage> messages = new List<ViberMessage>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(MessagesQuery))
            {
                _dataHelper.AddInParameter(sqlStringCommand, "cid", DbType.Int32, (object)conversationId);
                _dataHelper.AddInParameter(sqlStringCommand, "dtf", DbType.Int64, (object)tsFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "dtt", DbType.Int64, (object)tsTo);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    var row = tbl.Rows[index];
                    ViberMessage vm = new ViberMessage();

                    vm.ChatId = Convert.ToInt64(row["ChatId"]);
                    vm.Group = row["Group"].ToString();
                    vm.Timestamp = Convert.ToInt64(row["Timestamp"]);
                    int direction = Convert.ToInt32(row["Direction"]);

                    vm.Body = row["Body"].ToString();
                    vm.ThumbnailPath = row["ThumbnailPath"].ToString();
                    vm.PayloadPath = row["PayloadPath"].ToString();
                    vm.StickerId = Convert.ToInt64(row["StickerId"]);
                    vm.EventId = Convert.ToInt64(row["EventId"]);

                    if (direction == 0)
                    {
                        vm.Number = row["Number"].ToString();
                        vm.Name = row["Name"].ToString();
                        vm.ClientName = row["ClientName"].ToString();
                    }
                    else
                    {
                        vm.Name = "ME";
                        vm.Number = "ME";
                    }


                    messages.Add(vm);
                }
            }

            return messages;
        }

        internal List<ViberMessage> GetMessages(int idFrom, int maxRows)
        {
            List<ViberMessage> messages = new List<ViberMessage>();
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(MessagesQueryFrom))
            {                
                _dataHelper.AddInParameter(sqlStringCommand, "idf", DbType.Int64, (object)idFrom);
                _dataHelper.AddInParameter(sqlStringCommand, "maxRows", DbType.Int64, (object)maxRows);

                DataTable tbl = new DataTable();
                _dataHelper.FillTable(tbl, sqlStringCommand);

                for (int index = 0; index < tbl.Rows.Count; ++index)
                {
                    var row = tbl.Rows[index];
                    ViberMessage vm = new ViberMessage();

                    vm.ChatId = Convert.ToInt64(row["ChatId"]);
                    vm.Group = row["Group"].ToString();
                    vm.Timestamp = Convert.ToInt64(row["Timestamp"]);
                    int direction = Convert.ToInt32(row["Direction"]);

                    vm.Body = row["Body"].ToString();
                    vm.ThumbnailPath = row["ThumbnailPath"].ToString();
                    vm.PayloadPath = row["PayloadPath"].ToString();
                    vm.StickerId = Convert.ToInt64(row["StickerId"]);
                    vm.EventId = Convert.ToInt64(row["EventId"]);

                    if (direction == 0)
                    {
                        vm.Number = row["Number"].ToString();
                        vm.Name = row["Name"].ToString();
                        vm.ClientName = row["ClientName"].ToString();
                    }
                    else
                    {
                        vm.Name = "ME";
                        vm.Number = "ME";
                    }


                    messages.Add(vm);
                }
            }

            return messages;
        }

        public int GetMinMessageId()
        {
            int minId = -1;
            using (DbCommand sqlStringCommand = _dataHelper.GetSqlStringCommand(" SELECT Min(EventId) from main.Messages; "))
            {
                object result = _dataHelper.ExecuteScalar(sqlStringCommand);

                if (result != null)
                    minId = Convert.ToInt32(result);
            }

            return minId;
        }

        private string ConversationQuery = " SELECT DISTINCT CI.ChatID " +
" 	,CASE  " +
" 		WHEN CI.Flags = 64 AND C.Name <> ''" +
" 			THEN C.NAME " +
" 		ELSE CASE  " +
" 				WHEN CI.NAME = '' " +
" 					THEN CI.Token " +
" 				ELSE CI.NAME " +
" 				END " +
" 		END AS [Group] " +
" FROM ChatRelation CR " +
" INNER JOIN ChatInfo CI ON CR.ChatID = CI.ChatID " +
" INNER JOIN Contact C ON CR.ContactId = C.ContactID " +
" WHERE (C.EncryptedNumber <> ( " +
" 		SELECT SettingValue " +
" 		FROM settings " +
" 		WHERE SettingTitle = 'EncryptedPhoneNumber' " +
" 		) " +
" 	OR C.EncryptedNumber IS NULL) " +
" ORDER BY [Group]; ";


        private string MessagesQuery = " SELECT MSG.EventID, " +
"        MSG.Type, " +
"        MSG.Status, " +
"        MSG.Subject, " +
"        MSG.Body, " +
"        MSG.Flag, " +
"        MSG.PayloadPath, " +
"        MSG.ThumbnailPath, " +
"        MSG.StickerID, " +
"        MSG.PttID, " +
"        MSG.PttStatus, " +
"        MSG.Duration, " +
"        MSG.PGMessageId, " +
"        MSG.PGIsLiked, " +
"        MSG.PGLikeCount, " +
"        MSG.Info, " +
"        MSG.AppId, " +
"        EV.TimeStamp, " +
"        EV.Direction, " +
"        EV.ChatID, " +
"        C.Number, " +
"        C.Name, " +
"        C.ClientName, " +
"        CASE WHEN CI.Name = '' THEN CI.Token Else Ci.Name END AS[Group] " +
"   FROM main.Events EV " +
"        INNER JOIN " +
"        main.Messages MSG ON EV.EventId = MSG.EventId " +
"        INNER JOIN " +
"        main.ChatInfo CI ON EV.ChatID = CI.ChatID " +
"        INNER JOIN " +
"        main.Contact C ON C.ContactId = EV.ContactID " +
" WHERE CI.ChatID = :cid AND  " +
"        EV.TimeStamp >= :dtf AND " +
"        EV.TimeStamp <= :dtt; ";


        private string MessagesQueryFrom = " SELECT MSG.EventID, " +
"        MSG.Type, " +
"        MSG.Status, " +
"        MSG.Subject, " +
"        MSG.Body, " +
"        MSG.Flag, " +
"        MSG.PayloadPath, " +
"        MSG.ThumbnailPath, " +
"        MSG.StickerID, " +
"        MSG.PttID, " +
"        MSG.PttStatus, " +
"        MSG.Duration, " +
"        MSG.PGMessageId, " +
"        MSG.PGIsLiked, " +
"        MSG.PGLikeCount, " +
"        MSG.Info, " +
"        MSG.AppId, " +
"        EV.TimeStamp, " +
"        EV.Direction, " +
"        EV.ChatID, " +
"        C.Number, " +
"        C.Name, " +
"        C.ClientName, " +
"        CASE WHEN CI.Name = '' THEN CI.Token Else Ci.Name END AS[Group] " +
"   FROM main.Events EV " +
"        INNER JOIN " +
"        main.Messages MSG ON EV.EventId = MSG.EventId " +
"        INNER JOIN " +
"        main.ChatInfo CI ON EV.ChatID = CI.ChatID " +
"        INNER JOIN " +
"        main.Contact C ON C.ContactId = EV.ContactID " +
" WHERE MSG.EventID >= :idf LIMIT :maxRows; ";

        private string StatisticsQuery = " SELECT EV.ChatID " +
"            ,count(MSG.EventID) as [total_messages]	 " +
"              ,CASE " +
"                  WHEN EV.Direction <> 0 " +
"                      THEN 'ME' " +
"                  ELSE C.Name " +
"                  END AS [author] " +
" FROM main.Events EV " +
" INNER JOIN main.Messages MSG ON EV.EventId = MSG.EventId " +
" INNER JOIN main.ChatInfo CI ON EV.ChatID = CI.ChatID " +
" INNER JOIN main.Contact C ON C.ContactId = EV.ContactID " +
" WHERE CI.ChatID = :cid " +
" 	AND EV.TIMESTAMP >= :dtf " +
" 	AND EV.TIMESTAMP <= :dtt " +
"  GROUP BY [author] " +
"  ORDER BY total_messages DESC; ";

        private string StatisticsUrlQuery = " SELECT EV.ChatID " +
                                            "            ,count(MSG.EventID) as [total_messages]	 " +
                                            "              ,CASE " + "                  WHEN EV.Direction <> 0 " +
                                            "                      THEN 'ME' " + "                  ELSE C.Name " +
                                            "                  END AS [author] " + " FROM main.Events EV " +
                                            " INNER JOIN main.Messages MSG ON EV.EventId = MSG.EventId " +
                                            " INNER JOIN main.ChatInfo CI ON EV.ChatID = CI.ChatID " +
                                            " INNER JOIN main.Contact C ON C.ContactId = EV.ContactID " +
                                            " WHERE CI.ChatID = :cid " + " 	AND EV.TIMESTAMP >= :dtf " +
                                            " 	AND EV.TIMESTAMP <= :dtt " +
                                            $" 	AND MSG.BODY REGEXP  '{Constants.RegexUrl}'" + "  GROUP BY [author] " +
                                            "  ORDER BY total_messages DESC; ";
        
    }
}
