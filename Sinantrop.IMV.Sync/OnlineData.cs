using System;
using System.Collections.Generic;
using Sinantrop.IMV.Sync.Model;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.Sync
{
    [Serializable]
    public class OnlineData
    {        
        public ComputerInfo ComputerInfo { get; set; }
        public DateTime Created { get; set; }
        public List<Message> Messages { get; set; }
        public List<Conversation> Conversations { get; set; }
        public string Filename { get; set; }
        public string Username { get; set; }
        public int IdTo { get; set; }
        public int IdFrom { get; set; }
        public MessengerType MessengerType { get; set; }        
    }
}
