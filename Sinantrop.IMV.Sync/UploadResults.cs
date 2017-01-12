using System;
using Sinantrop.IMV.Sync.Model;

namespace Sinantrop.IMV.Sync
{
    public class UploadResults        
    {
        public bool IsCompletedSuccessfully { get; set; }
        public Exception Exception { get; set; }
        public UploadItem UploadItem { get; set; }
        public string OAuth { get; set; }
    }
}
