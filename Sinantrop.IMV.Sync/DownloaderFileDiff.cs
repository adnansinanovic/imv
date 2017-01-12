using Dropbox.Api.Files;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Sinantrop.DB.SQLite;
using Sinantrop.IMV.Sync.Model;

namespace Sinantrop.IMV.Sync
{
    public static class DownloaderFileDiff
    {
        public static async Task<IEnumerable<string>> ListDifference()
        {
            DropboxClient client = new DropboxClient(DropboxOAuthProvider.GetOauth());

            var dropboxFiles = await client.Files.ListFolderAsync(new ListFolderArg(Consts.DropboxPath));

            var localFiles = OrmEntity.FindAll<DownloadItem>(x => x.IsCompleted == false).ToList();

            var difference = dropboxFiles.Entries
                .Select(x => x.Name)
                .Except(localFiles.Select(x => x.Filename)).ToList();


            return difference;
        }
    }
}
