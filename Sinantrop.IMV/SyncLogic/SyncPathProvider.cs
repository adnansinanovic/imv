using System;
using System.IO;
using System.Security.Policy;

namespace Sinantrop.IMV.SyncLogic
{
    public class SyncPathProvider : IDbPathProvider
    {
        public string GetPath(string skypeUserName)
        {            
            return Path.Combine(GetPath(), $"{skypeUserName as object}\\main.db");
        }

        public string GetPath()
        {
            return Path.Combine(new Url(Environment.CurrentDirectory).Value, "_imvresults");
        }
    }
}
