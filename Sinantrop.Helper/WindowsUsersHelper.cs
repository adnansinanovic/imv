using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Microsoft.Win32;

namespace Sinantrop.Helper
{
    public static class WindowsUsersHelper
    {
        public static List<string> GetSids()
        {
            return Registry.Users.GetSubKeyNames()
                .Where(x => !x.Contains("DEFAULT") && !x.EndsWith("_Classes"))
                .ToList();
        }

        private static List<WindowsUser> GetWindowsUsers()
        {
            ManagementObjectSearcher usersSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_UserAccount");
            ManagementObjectCollection users = usersSearcher.Get();

            List<WindowsUser> accounts = new List<WindowsUser>();
            foreach (var o in users)
            {
                var user = (ManagementObject)o;
                var account = Activator.CreateInstance<WindowsUser>();

                typeof(WindowsUser)
                    .GetProperties()
                    .ToList()
                    .ForEach(property => property.SetValue(account, user[property.Name].ToString()));

                accounts.Add(account);
            }
            return accounts; 
        }

        public static List<string> GetAppData()
        {
            var results = new List<string>();
            var users = GetWindowsUsers().Where(u => u.Status == "OK").ToList();

            foreach (var user in users)
            {
                const string regValueAppData = @"AppData";
                string regKeyFolders = $@"HKEY_USERS\{user.SID}\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders";
                string appDataPath = Registry.GetValue(regKeyFolders, regValueAppData, string.Empty)?.ToString();

                if (appDataPath != null && !results.Contains(appDataPath))
                    results.Add(appDataPath);
            }

            return results;
        }

        private class WindowsUser
        {
            public string AccountType { get; set; }
            public string Caption { get; set; }
            public string Description { get; set; }
            public string Disabled { get; set; }
            public string Domain { get; set; }
            public string FullName { get; set; }
            public string LocalAccount { get; set; }
            public string Lockout { get; set; }
            public string Name { get; set; }
            public string PasswordChangeable { get; set; }
            public string PasswordExpires { get; set; }
            public string PasswordRequired { get; set; }
            public string SID { get; set; }
            public string SIDType { get; set; }
            public string Status { get; set; }
        }
    }   
}
