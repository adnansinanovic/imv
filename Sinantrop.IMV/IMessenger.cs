using System;
using System.Collections.Generic;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV
{
    public interface IMessenger
    {
        MessengerType MessengerType { get; }

        IEnumerable<string> GetUsernames();
        IEnumerable<Conversation> GetConversation(string user);
        IEnumerable<Message> GetMessages(string user, long conversationId, DateTime dtFrom, DateTime dtTo);     
      
        IEnumerable<ConversationStatistics> GetConversationStatistics(string user, long conversationId, DateTime dtFrom, DateTime dtTo, StatisticsType statisticsType);        
    }

    public interface IMessengerUploader : IMessenger
    {
        IEnumerable<Message> GetMessages(string user, int idFrom, int maxRows);

        int GetMinMessageId(string user);
    }
}
