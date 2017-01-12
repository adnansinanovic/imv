using Sinantrop.DB.SQLite;

namespace Sinantrop.IMV.Sync.Model
{
    public class DownloadItem : OrmEntity
    {        
        public bool IsCompleted { get; set; }
        public string Filename { get; set; }
    }
}
