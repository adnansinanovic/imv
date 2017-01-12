using Sinantrop.Logger.Appender;
using Sinantrop.Logger.Configuration;
using Sinantrop.Logger.ProducerConsumerPattern;

namespace Sinantrop.Logger
{
    public class StaticLogger
    {
        public static ILoggerConfiguration Configuration { get; set; } = new LoggerConfiguration();
        private static readonly ProducerConsumer<LogEvent> _producer;

        static StaticLogger()
        {
            _producer = new ProducerConsumer<LogEvent>(ItemProcessed);
        }

        private static void ItemProcessed(LogEvent obj)
        {
            foreach (IAppender appender in Configuration.Appenders)
            {
                appender.Enqueue(obj);
            }
        }

        public static void WriteLine<T>(T item)
        {
            AddToQueue(item);
        }

        private static void AddToQueue<T>(T item)
        {
            var log = new LogEvent<T>(item);         

            _producer.Add(log);
        }
    }
}
