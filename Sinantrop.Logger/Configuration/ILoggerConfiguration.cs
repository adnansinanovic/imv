using System.Collections.Generic;
using Sinantrop.Logger.Appender;

namespace Sinantrop.Logger.Configuration
{    
    public interface ILoggerConfiguration 
    {
        List<IAppender> Appenders { get; set; }
        void AddAppender(IAppender appender);

        void RemoveAppender<T>() where T : IAppender;
        void RemoveAppender(IAppender appender);
        void RemoveAppender(string appenderName);
        IAppender GetAppender(string appenderNamer);
        IEnumerable<T> GetAppender<T>() where T : IAppender;
    }
}
