using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sinantrop.IMV.SkypeLogic.Model;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.SkypeLogic
{
    public class Skype : IMessengerUploader
    {
        private readonly IDbPathProvider _pathProvider;
        private readonly SkypeDa _skypeDa;

        public Skype()
        {
            _pathProvider = new SkypePathProvider();
            _skypeDa = new SkypeDa();
        }

        public MessengerType MessengerType
        {
            get
            {
                return MessengerType.Skype;
            }
        }

        public IEnumerable<Conversation> GetConversation(string user)
        {
            //first we need set to which db we want to connect
            _skypeDa.SetDbPath(_pathProvider.GetPath(user));

            List<SkypeConversations> skypeConversations = _skypeDa.GetConversations();

            List<Conversation> result = new List<Conversation>();
            foreach (var item in skypeConversations)
            {
                Conversation c = SkypeConverter.GetConversation(item);
                c.Username = user;

                result.Add(c);
            }

            return result;
        }

        public IEnumerable<Message> GetMessages(string user, long conversationId, DateTime dtFrom, DateTime dtTo)
        {         
            //first we need set to which db we want to connect
            _skypeDa.SetDbPath(_pathProvider.GetPath(user));
          
            List<SkypeMessage> sMessages = _skypeDa.GetMessages(conversationId, dtFrom, dtTo);
            List<Message> messages = new List<Message>();
            foreach (SkypeMessage skypeMessage in sMessages)
            {
                Message message = SkypeConverter.GetMessage(skypeMessage);
                message.Username = user;
                messages.Add(message);
            }

            return messages;
        }


        public IEnumerable<Message> GetMessages(string user, int idFrom, int maxRows)
        {
            //first we need set to which db we want to connect
            _skypeDa.SetDbPath(_pathProvider.GetPath(user));

            List<SkypeMessage> sMessages = _skypeDa.GetMessages(idFrom, maxRows);

            List<Message> messages = new List<Message>();
            foreach (SkypeMessage skypeMessage in sMessages)
            {
                Message message = SkypeConverter.GetMessage(skypeMessage);
                message.Username = user;

                messages.Add(message);
            }

            return messages;
        }

        public IEnumerable<ConversationStatistics> GetConversationStatistics(string user, long conversationId, DateTime dtFrom, DateTime dtTo, StatisticsType statisticsType)
        {
            //first we need set to which db we want to connect
            _skypeDa.SetDbPath(_pathProvider.GetPath(user));

            List<SkypeConversationStatistics> items = statisticsType == StatisticsType.Full ? _skypeDa.GetStatistics(conversationId, dtFrom, dtTo) : _skypeDa.GetUrlStatistics(conversationId, dtFrom, dtTo);

            List<ConversationStatistics> results = new List<ConversationStatistics>();

            foreach (SkypeConversationStatistics item in items)
            {
                results.Add(new ConversationStatistics()
                {
                    ConversationId = (ulong)item.convo_id,
                    TotalMessages = item.total_messages,
                    Identity = item.author

                }
                );
            }

            return results;
        }

        public int GetMinMessageId(string user)
        {
            _skypeDa.SetDbPath(_pathProvider.GetPath(user));

            return _skypeDa.GetMinMessageId();
        }

        public IEnumerable<string> GetUsernames()
        {
            List<string> list = new List<string>();
            string path = _pathProvider.GetPath();

            foreach (string str in Directory.GetDirectories(path))
            {
                string directory = str.Replace(path, string.Empty).Replace("\\", string.Empty);
                if (!IgnoreDirectory(directory) && Enumerable.ToList(Directory.GetFiles(str, "*.db", SearchOption.TopDirectoryOnly)).Contains(Path.Combine(str, "main.db")))
                    list.Add(directory);
            }

            return list;
        }

        private bool IgnoreDirectory(string directory)
        {
            directory = directory.Replace("\\", string.Empty);
            foreach (var ignoreFolder in new[]
            {
                "Content", "DataRv", "My Skype Received Files", "shared_dynco", "shared_httpfe"
            })
            {
                if (ignoreFolder.ToLower() == directory.ToLower()) return true;
            }
            return false;
        }
    }
}
