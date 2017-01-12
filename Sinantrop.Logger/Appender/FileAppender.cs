using System;
using System.IO;
using System.Reflection;
using System.Text;
using Sinantrop.Logger.Helpers;

namespace Sinantrop.Logger.Appender
{
    public class FileAppender : BaseAppender
    {
        public string Filename { get; set; }
        public int MaxFileSizeMb { get; set; } = 5;
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public FileAppender()
        {
            Assembly assembly = Assembly.GetEntryAssembly();            

            Filename = $"{Path.GetDirectoryName(assembly.Location)}\\logs\\{AppDomain.CurrentDomain.FriendlyName}.log";
        }

        public override void Append(LogEvent logEvent)
        {
            var fileInfo = GetFile();

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new StreamWriter(fileStream, Encoding))
                {
                    Layout.Format(writer, logEvent);
                }
            }
        }

        private FileInfo GetFile()
        {
            FileInfo fileInfo = new FileInfo(Filename);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            if (fileInfo.Exists && fileInfo.Length > GetMaxSizeBytes())
                File.Delete(fileInfo.FullName);

            return fileInfo;
        }

        private int GetMaxSizeBytes()
        {
            return (int)ByteSize.FromMegaBytes(MaxFileSizeMb).Bytes;
        }       
    }
}
