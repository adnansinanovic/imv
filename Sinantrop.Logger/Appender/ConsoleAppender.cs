using System;
using Sinantrop.Logger.Rules;

namespace Sinantrop.Logger.Appender
{
    public class ConsoleAppender : BaseAppender
    {        
        public ConsoleAppender()
        {
            Rules.Add(new ConsoleRule());
        }

        public override void Append(LogEvent logEvent)
        {
            Layout.Format(Console.Out, logEvent);            
        }

        private class ConsoleRule : IRule
        {
            public bool IsValid(LogEvent logEvent)
            {
                return Environment.UserInteractive;
            }
        }
    }
}
