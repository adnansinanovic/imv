using System;
using Sinantrop.DB.SQLite;

namespace Sinantrop.IMV.Uploader.Service
{
    public class ErrorItem : OrmEntity
    {        
        public DateTime Created { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public MessengerType MessengerType { get; set; }                     
    }
}
