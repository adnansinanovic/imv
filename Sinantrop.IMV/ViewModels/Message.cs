using System;
using SQLite;


namespace Sinantrop.IMV.ViewModels
{
    [Serializable]
    public class Message
    {
        public DateTime Date
        {
            get
            {
                return Helper.UnixTimeStampToDateTime(Timestamp);
            }
        }

        /// <summary>
        /// Message is read from this account 
        /// (e.g. it's possible to have more than 1 skype account on one pc, 
        /// and this field tell us to which account message belongs)
        /// </summary>        
        public string Username { get; set; }

        /// <summary>
        /// User wrote wrote message, his account name. it's uniqe per user
        /// It cannot be changed.
        /// </summary>        
        public string Identity { get; set; }

        /// <summary>
        /// User who wrote message. This is what other users see, when they are logged in.
        /// User can change this name occassionaly. 
        /// It's more user-friendy than identity.
        /// </summary>        
        public string Author { get; set; }
        
        public string Content { get; set; }

        public string ConversationId { get; set; }
        
        [PrimaryKey]
        public long Id { get; set; }

        public MessengerType MessengerType { get; set; }

        public long Timestamp { get; set; }     
    }
}
