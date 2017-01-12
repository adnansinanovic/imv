using System;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace Sinantrop.IMV.ViberLogic
{
    public static class ViberPaths
    {
        public static string GetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Environment.UserInteractive)
                appData = WindowsUsersManager.GetAppData().First();

            return Path.Combine(new Url(appData).Value, "ViberPC");
        }

        public static string GetPath(string user)
        {
            return Path.Combine(GetPath(), $"{user}\\viber.db");
        }
    }
}
