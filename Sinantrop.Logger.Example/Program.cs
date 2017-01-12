using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Security;
using Microsoft.Win32;
using Sinantrop.Logger.Appender;
using Sinantrop.Logger.Example.Model;
using Sinantrop.Logger.Pattern;
using Sinantrop.Logger.Rules;

namespace Sinantrop.Logger.Example
{
    class Program
    {
        static void Main2(string[] args)
        {
            Animal animal = new Animal();
            animal.BirthDate = DateTime.Today.AddDays(-1);
            animal.Height = 45.2;
            animal.IsHappy = true;
            animal.Weight = 35.43m;
            animal.Nickname = "Trajvan";

            animal.Habitat = new Habitat
            {
                HabitatType = HabitatType.Islands,
                Description = "Habitat description goes here",
                Name = "Peter's island"
            };

            Logger logger = new Logger();
            FileAppender fileAppender = new FileAppender();
            fileAppender.Filename = @"C:\Folder\Logonja.log";

            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Rules.Add(new LogLevelEqualRule() { LogLevel = LogLevel.Error });

            logger.Configuration.AddAppender(fileAppender);
            logger.Configuration.AddAppender(consoleAppender);
            //logger.Configuration.AddAppender(new GmailSmtpAppender(new MailAddress("imvhelp@gmail.com", "IMV Help"), new MailAddress("imvhelp@gmail.com", "IMV Help"), "darkomarko22", "Log Message"));

            logger.WriteLine(animal, LogLevel.Error);
            //logger.WriteLine("Test test test");
            
            consoleAppender.Rules.Clear();
            consoleAppender.Layout.Header = null;


            logger.WriteLine("-------------------------------------------------");
            logger.WriteLine("-------------------------------------------------");

            const string regKeyFolders = @"HKEY_USERS\<SID>\Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders";
            const string regValueAppData = @"Local AppData";
            string[] keys = Registry.Users.GetSubKeyNames();
            List<String> paths = new List<String>();

            foreach (string sid in keys)
            {
                string sido = String.Copy(sid);

                if (sido.Contains("DEFAULT"))
                    continue;
            }

            Console.ReadKey();
        }
    }
}
