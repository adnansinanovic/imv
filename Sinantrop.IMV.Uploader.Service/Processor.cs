using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Sinantrop.DB.SQLite;
using Sinantrop.Helper;
using Sinantrop.IMV.Sync;
using Sinantrop.IMV.Sync.Model;
using Sinantrop.IMV.ViewModels;
using Sinantrop.Logger;

namespace Sinantrop.IMV.Uploader.Service
{
    internal class Processor
    {
        private readonly IMessengerUploader _messenger;

        public Processor(MessengerType messengerType)
        {
            _messenger = MessengerFactory.CreateUploader(messengerType);
        }

        public async Task<bool> Process()
        {
            UploaderLogger.Instance.WriteLine("Processing started...");

            List<string> usernames = _messenger.GetUsernames().ToList();

            UploaderLogger.Instance.WriteLine($"Total users to process: {usernames.Count}", 1);

            if (usernames.Any())
            {
                await Task.WhenAll(usernames.Select(ProcessUsername));
            }

            UploaderLogger.Instance.WriteLine("Processing done");

            return true;
        }

        private async Task ProcessUsername(string username)
        {
            UploaderLogger.Instance.WriteLine($"Processing: {username}", 2);

            int maxMessages = Config.GetMaxMessages();

            var oAuths = Config.GetOAuths();
            var idsFrom = GetFromIds(username, oAuths);

            UploaderLogger.Instance.WriteLine($"From - to for: {username}", 3, LogLevel.Debug);
            idsFrom.ForEach(i => UploaderLogger.Instance.WriteLine($"{i.Key} > {i.Value} - {maxMessages}", 4, LogLevel.Debug));

            IList<Conversation> conversations = _messenger.GetConversation(username).ToList();

            Dictionary<string, List<Message>> messages = idsFrom.ToDictionary(pair => pair.Key, pair => _messenger.GetMessages(username, pair.Value, maxMessages).ToList());

            UploaderLogger.Instance.WriteLine($"Messages count for: {username}", 3, LogLevel.Debug);
            messages.ForEach(i => UploaderLogger.Instance.WriteLine($"{i.Key} > {i.Value.Count} - {i.Value.Count == 0}", 4, LogLevel.Debug));

            Dictionary<string, UploadItem> uploadItems = CreateUploadItems(username, idsFrom, messages);

            OrmEntity.SaveEnumerable(uploadItems.Values.ToList());

            Dictionary<string, OnlineData> onlineData = uploadItems.ToDictionary(pair => pair.Key, pair => CreateOnlineData(username, uploadItems[pair.Key], messages[pair.Key], conversations));

            List<Sync.Uploader> uploaders = (from pair in idsFrom where messages[pair.Key].Any() select new Sync.Uploader(uploadItems[pair.Key], onlineData[pair.Key], pair.Key)).ToList();

            var results = await Task.WhenAll(uploaders.Select(uploader => uploader.Upload()));

            ProcessResults(results);
        }

        private void ProcessResults(UploadResults[] results)
        {
            foreach (UploadResults result in results)
            {
                var uploadItem = result.UploadItem;
                uploadItem.DtCompleted = DateTime.Now;
                uploadItem.Status = result.IsCompletedSuccessfully ? UploadItemStatus.Completed : UploadItemStatus.Failed;

                if (!result.IsCompletedSuccessfully)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"OAuth: {result.OAuth}");
                    sb.AppendLine($"Username: {result.UploadItem.Username}");
                    sb.AppendLine(result.Exception.GetString());
                    UploaderLogger.Instance.WriteException(result.Exception, _messenger.MessengerType);
                }
            }

            OrmEntity.SaveEnumerable(results.Select(x => x.UploadItem).ToList());
        }

        private Dictionary<string, UploadItem> CreateUploadItems(string username, Dictionary<string, int> idsFrom, Dictionary<string, List<Message>> messages)
        {
            Dictionary<string, UploadItem> u = new Dictionary<string, UploadItem>();
            foreach (KeyValuePair<string, int> pair in idsFrom)
            {
                if (messages[pair.Key].Any())
                {
                    var idTo = (int)messages[pair.Key].MaxBy(x => x.Id).Id;
                    var idFrom = pair.Value;
                    var oAuth = pair.Key;

                    var uploadItem = new UploadItem
                    {
                        DtStarted = DateTime.Now,
                        DtCompleted = DateTime.Now,
                        Status = UploadItemStatus.Processing,
                        Username = username,
                        IdFrom = idFrom,
                        IdTo = idTo,
                        OAuth = oAuth
                    };
                    uploadItem.Filename = $"{uploadItem.Username}_{uploadItem.IdFrom}_{uploadItem.IdTo}_{uploadItem.DtStarted:yyyyMMddHHmmssff}.imvdbf";

                    u.Add(pair.Key, uploadItem);
                }
                else
                    UploaderLogger.Instance.WriteLine($"No new messages. Username: {username}", 3);
            }

            return u;
        }

        private Dictionary<string, int> GetFromIds(string username, List<string> oAuths)
        {
            var idsFrom = new Dictionary<string, int>();

            foreach (string oAuth in oAuths)
            {
                Func<UploadItem, bool> predicate1 = x => x.Username == username && x.Status == UploadItemStatus.Completed && x.OAuth == oAuth;
                Func<UploadItem, bool> predicate2 = x => x.Username == username && x.Status != UploadItemStatus.Completed && x.OAuth == oAuth;

                //var minBy = OrmEntity.MaxBy(predicate1, x => x.IdTo);
                //var maxBy = OrmEntity.MinBy(predicate2, x => x.IdFrom);
                //var id = _messenger.GetMinMessageId(username);
                //int idFrom = minBy?.IdTo ?? maxBy?.IdFrom ?? id;

                int idFrom = OrmEntity.MaxBy(predicate1, x => x.IdTo)?.IdTo ??
                             OrmEntity.MinBy(predicate2, x => x.IdFrom)?.IdFrom ??
                             _messenger.GetMinMessageId(username);

                idsFrom.Add(oAuth, idFrom + 1);
            }
            return idsFrom;
        }

        private OnlineData CreateOnlineData(string username, UploadItem uploadItem, IList<Message> messages, IList<Conversation> conversations)
        {
            OnlineData onlineData = new OnlineData
            {
                Created = uploadItem.DtStarted,
                Messages = messages.ToList(),
                Conversations = conversations.ToList(),
                Filename = uploadItem.Filename,
                Username = username,
                IdFrom = uploadItem.IdFrom,
                IdTo = uploadItem.IdTo,
                MessengerType = _messenger.MessengerType,
                ComputerInfo = ComputerInfoHolder.Info
            };

            return onlineData;
        }
    }
}
