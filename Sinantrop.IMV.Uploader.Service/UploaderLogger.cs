using System;
using System.Diagnostics;
using System.Net.Mail;
using Sinantrop.DB.SQLite;
using Sinantrop.Helper;
using Sinantrop.Logger;
using Sinantrop.Logger.Appender;
using Sinantrop.Logger.Layout;
using Sinantrop.Logger.Rules;

namespace Sinantrop.IMV.Uploader.Service
{
    internal class UploaderLogger
    {
        private static UploaderLogger _instance;
        private static Logger.Logger _realLogger;

        public static UploaderLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UploaderLogger();
                    _realLogger = new Logger.Logger();


                    ILayout layout = new SimpleInlineLayout();


                    IAppender gmailAppender = new GmailSmtpAppender(new MailAddress("imvhelp@gmail.com", "IMV Help"),
                        new MailAddress("imvhelp@gmail.com", "IMV Help"), "darkomarko22", "Log Message");
                    gmailAppender.Rules.Add(new MailRule());

                    IAppender fileAppender = new FileAppender();
                    fileAppender.Layout = layout;

                    IAppender consoleAppender = new ConsoleAppender();
                    consoleAppender.Layout = layout;
                    
                    _realLogger.Configuration.AddAppender(consoleAppender);
                    _realLogger.Configuration.AddAppender(fileAppender);
                    _realLogger.Configuration.AddAppender(gmailAppender);

                    if (!Debugger.IsAttached)
                    {
                        IRule debugRule = new LogLeveGreaterThanRule() {LogLevel = LogLevel.Debug};
                        foreach (IAppender appender in _realLogger.Configuration.Appenders)
                        {
                            appender.Rules.Add(debugRule);
                        }
                    }
                }

                return _instance;
            }
        }

        public void WriteLine(string msg, int level = 0, LogLevel logLevel = LogLevel.Information, MessengerType messengerType = MessengerType.Unknown)
        {
            msg = $"{string.Empty.PadRight(level, ' ')}{msg}";

            _realLogger.Add(new UploaderLogItem(msg, messengerType, logLevel));
        }

        public void WriteException(Exception exception, MessengerType messengerType = MessengerType.Unknown)
        {
            _realLogger.Add(new UploaderLogItem(exception.GetString(), messengerType, LogLevel.Error));
        }

        private class UploaderLogItem : LogEvent<string>
        {
            public UploaderLogItem(string logobj, MessengerType messengerType, LogLevel loglevel) : base(logobj)
            {
                MessengerType = messengerType;
                Level = loglevel;
            }

            public MessengerType MessengerType { get; set; }            
        }

        private class MailRule : IRule
        {
            /// <summary>
            /// Do not save same exception more than once in 24 hours
            /// </summary>
            /// <param name="logEvent"></param>
            /// <returns></returns>
            public bool IsValid(LogEvent logEvent)
            {
                UploaderLogItem ex = logEvent as UploaderLogItem;

                //if we are debuggind, do not save it
                if (Debugger.IsAttached)
                    return false;

                if (logEvent.Level < LogLevel.Error)
                    return false;

                //if its not exception, save it
                if (ex == null)
                    return true;

                //if it's from unknown messenger, save it
                if (ex.MessengerType == MessengerType.Unknown)
                    return true;                

                string v = ex.GetLogObject().ToString();

                Func<ErrorItem, bool> predicate = x =>
                        x.Message == v
                        && x.MessengerType == ex.MessengerType
                        && x.Created.AddDays(1) > DateTime.Now;
                
                var errorItem = OrmEntity.MaxBy(predicate, x => x.Id);

                //if there was same exception in last 24 hours, do not save it
                bool isValid = errorItem == null;

                if (isValid)
                {
                    ErrorItem error = new ErrorItem();
                    error.Message = ex.GetLogObject().ToString();
                    error.MessengerType = ex.MessengerType;
                    OrmEntity.Save(error);
                }

                return isValid;
            }
        }
    }
}
