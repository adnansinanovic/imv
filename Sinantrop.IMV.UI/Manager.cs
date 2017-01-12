using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sinantrop.IMV.SkypeLogic;
using Sinantrop.IMV.SyncLogic;
using Sinantrop.IMV.ViberLogic;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.UI
{
    internal class Manager
    {
        public static int MaxMessages = 1000;
        private readonly SortedDictionary<MessengerType, IMessenger> _messengers;

        private static Manager _instance;

        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Manager();

                return _instance;
            }
        }

        private Manager()
        {
            _messengers = new SortedDictionary<MessengerType, IMessenger>();

            Reinitialize();
        }

        public void Reinitialize()
        {
            _messengers.Clear();

            SkypePathProvider skypePathProvider = new SkypePathProvider();
            if (Directory.Exists(skypePathProvider.GetPath()))
                _messengers.Add(MessengerType.Skype, MessengerFactory.Create(MessengerType.Skype));
         
            if (Viber.IsInstalled())
                _messengers.Add(MessengerType.Viber, MessengerFactory.Create(MessengerType.Viber));

            SyncPathProvider syncPaths = new SyncPathProvider();
            if (Directory.Exists(syncPaths.GetPath()))
                _messengers.Add(MessengerType.Sync, MessengerFactory.Create(MessengerType.Sync));
        }

        public List<UiUser> GetUsers()
        {
            List<UiUser> users = new List<UiUser>();
            foreach (IMessenger messenger in _messengers.Values)
            {
                IEnumerable<string> mUsers = messenger.GetUsernames();

                foreach (string user in mUsers)
                {
                    users.Add(new UiUser(user, messenger.MessengerType));
                }
            }

            return users;
        }

        public List<Conversation> GetConversation(UiUser user)
        {
            List<Conversation> conversations = new List<Conversation>();

            foreach (var item in _messengers.Values)
            {
                if (user.Messenger == item.MessengerType)
                {
                    var c = item.GetConversation(user.Username).ToList();
                    c.ForEach(x => x.MessengerType = item.MessengerType);
                    conversations.AddRange(c);
                }
            }

            return conversations;
        }

        public List<Message> GetMessages(Conversation conversation, DateTime dtFrom, DateTime dtTo)
        {
            IMessenger messenger = _messengers[conversation.MessengerType];
       
            var items = messenger.GetMessages(conversation.Username, conversation.Id, dtFrom, dtTo).ToList();

            return items.Take(MaxMessages).ToList();
        }      
        public List<ConversationStatistics> GetStatistics(Conversation conversation, DateTime dtFrom, DateTime dtTo, StatisticsType statisticsType)
        {
            IMessenger messenger = _messengers[conversation.MessengerType];

            IEnumerable<ConversationStatistics> result = messenger.GetConversationStatistics(conversation.Username, conversation.Id, dtFrom, dtTo, statisticsType);

            return result.ToList();
        }
    }
}
