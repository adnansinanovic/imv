using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using Sinantrop.Db.SQLite;
using Sinantrop.Db.SQLite.TableCreators;
using Sinantrop.DB.SQLite;
using Sinantrop.IMV.ViewModels;

namespace Sinantrop.IMV.Sync
{
    public class Downloader
    {
        private bool _isDownloading;
        private int _totalDownloadedFiles;
        private readonly List<string> _dropboxFiles;
        private readonly Action<CallbackArgs> _progressNotification;
        private readonly Compression _compression = new Compression();

        public Downloader(List<string> dopboxFiles, Action<CallbackArgs> progressNotification = null)
        {
            _dropboxFiles = dopboxFiles;
            _progressNotification = progressNotification;
        }

        public void StartDownloading()
        {
            if (_isDownloading)
                return;

            _totalDownloadedFiles = 0;
            _isDownloading = true;

            _progressNotification?.Invoke(new CallbackArgs() { Message = $"Connecting to dropbox..." });
            DropboxClient client = new DropboxClient(DropboxOAuthProvider.GetOauth());

            if (!_dropboxFiles.Any())
            {
                _progressNotification?.Invoke(new CallbackArgs()
                {
                    CurrentItem = 0,
                    TotalItems = _dropboxFiles.Count,
                    Message = "Nothing to download"
                });
            }
            else
            {
                foreach (string file in _dropboxFiles)
                {
                    _progressNotification?.Invoke(new CallbackArgs() { Message = $"Downloading started..." });
                    DownloadArg arg = new DownloadArg($"{Consts.DropboxPath}{file}");
                    client.Files.BeginDownload(arg, DownloadCallback);
                }
            }
        }

        private async void DownloadCallback(IAsyncResult ar)
        {
            var result = ar as Task<IDownloadResponse<FileMetadata>>;
            if (result != null)
            {
                //download encrypted file                             
                byte[] cipher = await result.Result.GetContentAsByteArrayAsync();

                //decrypt file
                using (MemoryStream zipped = new MemoryStream(Encryption.Decrypt(cipher)))
                {
                    //unzip file
                    using (MemoryStream unzippedStream = _compression.Unzip(zipped))
                    {
                        //deserialize
                        unzippedStream.Position = 0;
                        IFormatter formatter = new BinaryFormatter();

                        OnlineData onlineData = (OnlineData)formatter.Deserialize(unzippedStream);

                        Save(onlineData);
                    }
                }
            }

            _totalDownloadedFiles++;

            _progressNotification?.Invoke(new CallbackArgs()
            {
                CurrentItem = _totalDownloadedFiles,
                TotalItems = _dropboxFiles.Count,
                Message = _totalDownloadedFiles < _dropboxFiles.Count ?
                    $"Downloading {_totalDownloadedFiles}/{_dropboxFiles.Count}" :
                    $"Download completed {_totalDownloadedFiles}/{_dropboxFiles.Count}"
            });
        }

        private void Save(OnlineData onlineData)
        {


            DirectoryInfo dir = new DirectoryInfo($"_imvresults/{onlineData.Username}");
            if (!dir.Exists)
                dir.Create();

            OrmContextSettings settings = new OrmContextSettings();
            settings.DbName = Path.Combine(dir.FullName, "main.db");
            settings.ContextName = dir.FullName;
            settings.TableCreators = new List<ITableCreator>()
            {
                new FromTypes(typeof(Conversation), typeof(Message))
            };

            OrmContext context = OrmEnvironment.Instance.AddContextIfNotExists(settings, false);

            OrmEntity.SaveEnumerable(onlineData.Conversations, context);
            OrmEntity.SaveEnumerable(onlineData.Messages, context);

        }
    }
}
