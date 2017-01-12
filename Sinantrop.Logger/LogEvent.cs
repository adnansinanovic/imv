using System;
using System.Threading;

namespace Sinantrop.Logger
{
    public abstract class LogEvent 
    {
        public int ManagedThreadId { get; set; }
        public string ThreadName { get; set; }

        public DateTime Created { get; private set; }

        public LogLevel Level { get; set; }

        protected LogEvent()
        {            
            Created = DateTime.Now;
            ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
        }

        public abstract object GetLogObject();
    }

    public class LogEvent<T> : LogEvent
    {
        private readonly T _logobj;

        public LogEvent(T logobj)
        {
            _logobj = logobj;          
        }

        public override object GetLogObject()
        {
            return _logobj;
        }
    }
}
