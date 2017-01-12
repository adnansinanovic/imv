using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinantrop.IMV.Sync
{
    public static class DropboxOAuthProvider
    {
        private static string _injectedOauth;

        public static void Inject(string outh)
        {
            _injectedOauth = outh;
        }

        public static string GetOauth()
        {
            if (!string.IsNullOrWhiteSpace(_injectedOauth))
                return _injectedOauth;

            return ConfigurationManager.AppSettings["DB_APP"];        
        }
    }
}
