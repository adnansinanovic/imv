using System;
using Sinantrop.DB.SQLite;

namespace Sinantrop.IMV.Sync.Model
{
    public class UploadItem : OrmEntity
    {        
        public string Username { get; set; }
        public int IdFrom { get; set; }
        public int IdTo { get; set; }
        public UploadItemStatus Status { get; set; }
        public string Filename { get; set; }
        public DateTime DtStarted { get; set; }
        public DateTime DtCompleted { get; set; }
        public string OAuth { get; set; }

    }

    public enum UploadItemStatus
    {
        Unknown,
        Processing,
        Completed,
        Failed
    }
}
