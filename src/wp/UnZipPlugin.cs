using System;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Windows.Storage;



namespace WPCordovaClassLib.Cordova.Commands
{
    public class UnZipPlugin : BaseCommand
    {       
        public void UnZip(string options)
        {

            string myPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\philipfiles";
            DirectoryInfo directorySelected = new DirectoryInfo(myPath);
            

            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.zip"))
            {
                Decompress(fileToDecompress);
            }
        }
       
        public void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                PluginResult result;
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                try
                {
                    ExtractToDirectory(currentFileName, newFileName);
                    result = new PluginResult(PluginResult.Status.OK, fileToDecompress.Name);
                }
                catch (Exception)
                {
                    result = new PluginResult(PluginResult.Status.ERROR, fileToDecompress.Name);
                }
                DispatchCommandResult(result);          
            }
        }

        public void ExtractToDirectory(string zipPath, string destination)
        {
            FileStream stream = new FileStream(zipPath, FileMode.Open, FileAccess.Read);
            ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            string myPath = localFolder.Path + "\\philipfiles";
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
           
            DirectoryInfo directorySelected = new DirectoryInfo(myPath);

            string fullName = directorySelected.FullName;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                string fullPath = Path.GetFullPath(Path.Combine(fullName, entry.FullName));

                if (Path.GetFileName(fullPath).Length == 0)
                {
                    if (entry.Length != 0L)
                    {
                        throw new IOException();
                    }
                    isoStore.CreateDirectory(fullPath);
                }
                else
                {
                    isoStore.CreateDirectory(Path.GetDirectoryName(fullPath));
                    ExtractToFile(isoStore, entry, fullPath);
                }
            }
        }

        public void ExtractToFile(IsolatedStorageFile dataFolder, ZipArchiveEntry entry, string destinationFileName)
        {
          

            using (Stream stream = dataFolder.CreateFile(destinationFileName))
            {
                using (Stream stream2 = entry.Open())
                {
                    stream2.CopyTo(stream);
                }
            }
        }
    }
}
