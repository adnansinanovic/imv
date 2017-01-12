using System.Diagnostics;

namespace Sinantrop.IMV.Sync
{
    public class CallbackArgs
    {
        [DebuggerStepThrough]
        public CallbackArgs()
        {
            CurrentItem = -1;
            TotalItems = -1;
        }

        public string Message { get; set; }
        public int TotalItems { get; set; }
        public int CurrentItem { get; set; }
    }
}
