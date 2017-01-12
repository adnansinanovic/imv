using System;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace Sinantrop.IMV.SkypeLogic
{
    public class SkypePathProvider : IDbPathProvider
    {
        public string GetPath(string skypeUserName)
        {
            return Path.Combine(GetPath(), $"{(object) skypeUserName}\\main.db");
        }

        public string GetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Environment.UserInteractive)
                appData = WindowsUsersManager.GetAppData().First();

            return Path.Combine(new Url(appData).Value, "Skype");            
        }       
    }
}
