
using System;
using SQLite;

namespace Sinantrop.IMV.ViewModels
{
    [Serializable]
    public class Conversation 
    {
        public Conversation()
        {
            Title = string.Empty;
            Username = string.Empty;
        }

        /// <summary>
        /// Group, conversation,
        /// Can be more authoros in one conversation
        /// </summary>
        public string Title { get; set; }

        [PrimaryKey]
        public long Id { get; set; }

        public MessengerType MessengerType { get; set; }

        public string Username { get; set; }
    }
}
