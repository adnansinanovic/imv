using System;
using System.Collections.Generic;
using System.IO;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.SyncLogic
{
    public class Sync : IMessenger
    {
        private readonly IDbPathProvider _pathProvider = new SyncPathProvider();
        private readonly SyncDa _dataAccess = new SyncDa();

        public MessengerType MessengerType => MessengerType.Sync;

        public IEnumerable<string> GetUsernames()
        {
            var usernames = Directory.GetDirectories(_pathProvider.GetPath());
            foreach (var username in usernames)
            {
                yield return Path.GetFileName(username);
            }
        }

        public IEnumerable<Conversation> GetConversation(string user)
        {


            _dataAccess.SetDbPath(_pathProvider.GetPath(user));

            return _dataAccess.GetConversations();
        }

        public IEnumerable<Message> GetMessages(string user, long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            _dataAccess.SetDbPath(_pathProvider.GetPath(user));

            return _dataAccess.GetMessages(conversationId, dtFrom, dtTo);
        }

        public IEnumerable<ConversationStatistics> GetConversationStatistics(string user, long conversationId, DateTime dtFrom, DateTime dtTo, StatisticsType statisticsType)
        {
            _dataAccess.SetDbPath(_pathProvider.GetPath(user));
            return statisticsType == StatisticsType.Full ? _dataAccess.GetStatistics(conversationId, dtFrom, dtTo) : _dataAccess.GetUrlStatistics(conversationId, dtFrom, dtTo);
        }
    }
}
