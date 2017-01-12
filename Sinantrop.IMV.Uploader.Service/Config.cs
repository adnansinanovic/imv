using System.Collections.Generic;
using System.Configuration;
using Sinantrop.IMV.Sync;

namespace Sinantrop.IMV.Uploader.Service
{
    internal static class Config
    {
        public static int GetTaskIntervalMiliseconds()
        {
            const int shortInterval = 10; //10 seconds
            const int longInterval = 60 * 60; //1 hour = 60 sec * 60 min

            string intervalKey = System.Diagnostics.Debugger.IsAttached ? "TASK_INTERVAL_DEBUG" : "TASK_INTERVAL";
            int defaultIntervalSeconds = System.Diagnostics.Debugger.IsAttached ? shortInterval : longInterval;

            return GetIntConfig(intervalKey, defaultIntervalSeconds) * 1000;
        }

        public static int GetMaxMessages()
        {
            return GetIntConfig("MAX_MESSAGES", 20000);
        }

        private static int GetIntConfig(string key, int defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];

            int result;
            if (!int.TryParse(setting, out result))
                result = defaultValue;

            if (result <= 0)
                result = defaultValue;

            return result;
        }

        public static List<string> GetOAuths()
        {
            List<string> result = new List<string>();
            string app = DropboxOAuthProvider.GetOauth();

            if (!string.IsNullOrWhiteSpace(app))
                result.Add(app);

            return result;
        }
    }
}
