using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using Dropbox.Api;
using Sinantrop.IMV.Sync.Model;

namespace Sinantrop.IMV.Sync
{
    public class Uploader
    {
        private UploadItem UploadItem { get; set; }
        private readonly OnlineData _onlineData;
        private readonly string _oatuh;
        private readonly Compression _compression = new Compression();
        
        public Uploader(UploadItem uploadItem, OnlineData onlineData, string oatuh)
        {
            UploadItem = uploadItem;
            _onlineData = onlineData;
            _oatuh = oatuh;
        }     
        public async Task<UploadResults> Upload()
        {
            return await Upload(UploadItem, _onlineData, _oatuh);            
        }

        private async Task<UploadResults> Upload(UploadItem uploadItem, OnlineData onlineData, string oauth)
        {
            UploadResults results = new UploadResults();
            results.UploadItem = uploadItem;
            results.OAuth = oauth;

            /*
             * Steps:
             * 1 - serialize
             * 2 - zip
             * 3 - encrypt
             * 4 - upload 
             */
            try
            {
                using (MemoryStream memoryPlainText = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryPlainText, onlineData);

                    using (MemoryStream zippedStream = _compression.Zip(memoryPlainText))
                    {
                        byte[] cipher = Encryption.Encrypt(zippedStream.ToArray());
                        using (MemoryStream cipherText = new MemoryStream(cipher))
                        {
                            cipherText.Seek(0, SeekOrigin.Begin);

                            string destination = $"{Consts.DropboxPath}{uploadItem.Filename}";
                            CommitInfo commitInfo = new CommitInfo(destination, WriteMode.Overwrite.Instance, mute: true);
                            DropboxClient client = new DropboxClient(oauth);                          

                            await client.Files.UploadAsync(commitInfo, cipherText);

                            results.IsCompletedSuccessfully = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                results.IsCompletedSuccessfully = false;
                results.Exception = ex;
            }


            return results;
        }      
    }
}
