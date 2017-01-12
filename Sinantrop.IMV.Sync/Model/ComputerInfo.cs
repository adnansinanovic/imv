using System;
using SQLite;

namespace Sinantrop.IMV.Sync.Model
{
    [Serializable]
    public class ComputerInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string Guid { get; set; }
        public string WindowsUserName { get; set; }
        public string WindowsVersion { get; set; }
    }
}
