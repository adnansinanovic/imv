using System;
using System.Text;
using System.Threading;
using Sinantrop.Logger.Appender;
using Sinantrop.Logger.Configuration;
using Sinantrop.Logger.ProducerConsumerPattern;

namespace Sinantrop.Logger
{
    public class Logger
    {
        public ILoggerConfiguration Configuration { get; set; } = new LoggerConfiguration();        
        private readonly ProducerConsumer<LogEvent> _producer;

        public Logger()
        {
            _producer = new ProducerConsumer<LogEvent>(ItemProcessed);
        }

        private void ItemProcessed(LogEvent obj)
        {
            foreach (IAppender appender in Configuration.Appenders)
            {
                appender.Enqueue(obj);
            }
        }

        public void WriteLine<T>(T item, LogLevel level = LogLevel.Information)
        {                   
            var log = new LogEvent<T>(item)
            {
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                Level = level
            };

            Add(log);
        }

        public void Add<T>(LogEvent<T> logevent)
        {
            _producer.Add(logevent);
        }              
    }
}
