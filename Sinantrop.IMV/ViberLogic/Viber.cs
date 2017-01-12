using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sinantrop.IMV.ViberLogic.Model;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.ViberLogic
{
    public class Viber : IMessengerUploader
    {
        public static bool IsInstalled()
        {
            return new Viber().GetUsernames().Any();
        }

        private readonly ViberDA _viberDa = new ViberDA();
        public MessengerType MessengerType => MessengerType.Viber;

        public IEnumerable<Conversation> GetConversation(string user)
        {
            _viberDa.SetDbPath(ViberPaths.GetPath(user));
            var items = _viberDa.GetConversations();

            List<Conversation> result = new List<Conversation>();
            foreach (ViberConversation item in items)
                result.Add(new Conversation() { Id = item.ChatId, Title = item.Group, MessengerType = MessengerType.Viber, Username = user });

            return result;
        }

        public IEnumerable<ConversationStatistics> GetConversationStatistics(string user, long conversationId, DateTime dtFrom, DateTime dtTo, StatisticsType statisticsType)
        {
            _viberDa.SetDbPath(ViberPaths.GetPath(user));
            var items = statisticsType == StatisticsType.Full ? _viberDa.GetStatistics(conversationId, dtFrom, dtTo) : _viberDa.GetUrlStatistics(conversationId, dtFrom, dtTo);

            List<ConversationStatistics> results = new List<ConversationStatistics>();

            foreach (ViberConversationStatistics item in items)
            {
                results.Add(new ConversationStatistics()
                {
                    ConversationId = (ulong)item.chatId,
                    TotalMessages = item.total_messages,
                    Identity = item.author

                }
                );
            }

            return results;
        }

        public int GetMinMessageId(string user)
        {
            _viberDa.SetDbPath(ViberPaths.GetPath(user));

            return _viberDa.GetMinMessageId();
        }

        public IEnumerable<Message> GetMessages(string user, int idFrom, int maxRows)
        {
            _viberDa.SetDbPath(ViberPaths.GetPath(user));

            List<ViberMessage> viberMessages = _viberDa.GetMessages(idFrom, maxRows);

            foreach (ViberMessage viberMessage in viberMessages)
            {
                Message message = ViberConverter.GetMessage(viberMessage);
                message.Username = user;
                yield return message;
            }
        }

        public IEnumerable<Message> GetMessages(string user, long conversationId, DateTime dtFrom, DateTime dtTo)
        {
            //first we need set to which db we want to connect
            _viberDa.SetDbPath(ViberPaths.GetPath(user));
            List<ViberMessage> viberMessages = _viberDa.GetMessages(conversationId, dtFrom, dtTo);

            foreach (var item in viberMessages)
            {
                Message m = ViberConverter.GetMessage(item);
                m.Username = user;
                yield return m;

            }
        }

        public IEnumerable<string> GetUsernames()
        {
            List<string> list = new List<string>();
            string path = ViberPaths.GetPath();

            if (Directory.Exists(path))
            {
                foreach (string str in Directory.GetDirectories(path))
                {
                    string directory = str.Replace(path, string.Empty).Replace("\\", string.Empty);
                    if (!IgnoreDirectory(directory) &&
                        Directory.GetFiles(str, "*.db", SearchOption.TopDirectoryOnly).ToList().Contains(Path.Combine(str, "viber.db")))
                        list.Add(directory);
                }
            }
            return list;
        }

        private bool IgnoreDirectory(string directory)
        {
            directory = directory.Replace("\\", string.Empty);

            return new[]
            {
                "data"
            }.Any(ignoreFolder => ignoreFolder.ToLower() == directory.ToLower() || ignoreFolder.Contains("."));
        }
    }
}
