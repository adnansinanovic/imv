using System.Collections.Generic;
using System.Linq;
using Sinantrop.Logger.Layout;
using Sinantrop.Logger.ProducerConsumerPattern;
using Sinantrop.Logger.Rules;

namespace Sinantrop.Logger.Appender
{
    public abstract class BaseAppender : IAppender
    {
        public List<IRule> Rules { get; set; } = new List<IRule>();

        private ProducerConsumer<LogEvent> _queue;

        public ILayout Layout { get; set; } = new SimpleLayout();

        public bool UseSeparateThread { get; set; }

        protected BaseAppender()
        {
            Name = GetType().Name;

        }
      
        public string Name { get; set; }
        public abstract void Append(LogEvent logEvent);

        public void Enqueue(LogEvent logEvent)
        {
            if (!ProcessRules(logEvent))
                return;

            if (UseSeparateThread)
                (_queue ?? (_queue = new ProducerConsumer<LogEvent>(Append, Name))).Add(logEvent);
            else
                Append(logEvent);
        }

        private bool ProcessRules(LogEvent logEvent)
        {
            return Rules.All(rule => rule.IsValid(logEvent));
        }
    }
}
