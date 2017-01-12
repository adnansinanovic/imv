using System;
using System.Collections.Generic;
using System.Linq;
using Sinantrop.Logger.Appender;

namespace Sinantrop.Logger.Configuration
{
    public class LoggerConfiguration : ILoggerConfiguration
    {
        public List<IAppender> Appenders { get; set; } = new List<IAppender>();

        public void AddAppender(IAppender appender)
        {            
            if (Appenders.Any(x => x == appender || x.Name == appender.Name))
                throw new Exception($"Unable to add appender. Appender with '{appender.Name}' of type '{appender.GetType().Name}' already exists.");

            Appenders.Add(appender);
        }

        public void RemoveAppender(IAppender appender)
        {
            Appenders.Remove(appender);
        }

        public void RemoveAppender<T>() where T : IAppender
        {
            Appenders.OfType<T>().ToList().ForEach(x => Appenders.Remove(x));
        }

        public void RemoveAppender(string appenderName)
        {
            IAppender appender = GetAppender(appenderName);

            RemoveAppender(appender);
        }

        public IAppender GetAppender(string appenderName)
        {
            Func<IAppender, bool> predicate = x => x.Name.Equals(appenderName, StringComparison.Ordinal);

            if (!Appenders.Any(predicate))
                return null;

            return Appenders.First(predicate);            
        }     

        public IEnumerable<T> GetAppender<T>() where T : IAppender
        {
            return Appenders.OfType<T>();
        }
    }
}
