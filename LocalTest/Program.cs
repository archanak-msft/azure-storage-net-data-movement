using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.DataMovement;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LocalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;

            CloudStorageAccount saSource = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;FileEndpoint=https://archanak2.file.core.windows.net/;AccountName=archanak2;AccountKey=fiV8IhzvX0TY6R8GE0JIRdTPPvbpGIhixpvticUIjnXTSH9xpmHb2CqQwuCQBsB+er+hgMfGd77wSZFmc97Lxg==");
            CloudFileClient cloudFileClient = saSource.CreateCloudFileClient();
            CloudFileShare sourceCloudFileShare = cloudFileClient.GetShareReference("archanak21");
            sourceCloudFileShare.CreateIfNotExists();
            CloudFileDirectory sourceDir = sourceCloudFileShare.GetRootDirectoryReference();
            sourceDir.CreateIfNotExists();

            //IEnumerable<CloudFileShare> shareList = cloudFileClient.ListShares();

            //foreach (CloudFileShare fileShare in shareList)
            //{
            //    fileShare.FetchAttributes();
            //    Microsoft.WindowsAzure.Storage.File.Protocol.ShareStats statsSource = fileShare.GetStats();
            //    Console.WriteLine("Size of {0} = {1} ", fileShare.Name, statsSource.Usage);

            //    Console.WriteLine();
            //}

            CloudStorageAccount saDest = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;FileEndpoint=https://archanak2.file.core.windows.net/;AccountName=archanak2;AccountKey=fiV8IhzvX0TY6R8GE0JIRdTPPvbpGIhixpvticUIjnXTSH9xpmHb2CqQwuCQBsB+er+hgMfGd77wSZFmc97Lxg==");
            CloudFileClient cloudFileClientDest = saDest.CreateCloudFileClient();
            CloudFileShare destCloudFileShare = cloudFileClientDest.GetShareReference("archanak-s10");
            destCloudFileShare.CreateIfNotExists();
            CloudFileDirectory destDir = destCloudFileShare.GetRootDirectoryReference();
            destDir.CreateIfNotExists();


            TransferManager.Configurations.ParallelOperations = 64;

            TransferContext context = new TransferContext();
            context.ShouldTransferCallback = (source, destination) =>
            {
                Console.WriteLine("{0}  \n  {1}? \n\n\n", destination, source);
                return false;
            };
            context.FileFailed += FileSkippedCallback;


            context.ProgressHandler = new Progress<TransferStatus>((progress) =>
            {
                Console.WriteLine("Bytes uploaded: {0}", progress.BytesTransferred);
                Console.WriteLine("Files transferred: {0}", progress.NumberOfFilesTransferred);
                Console.WriteLine("Files failed: {0}", progress.NumberOfFilesFailed);
                Console.WriteLine("Files skipped: {0}", progress.NumberOfFilesSkipped);
                Console.WriteLine("*****************************");
                Console.WriteLine();
            });

            CopyDirectoryOptions options = new CopyDirectoryOptions()
            {
                Recursive = true,
            };

            var task = TransferManager.CopyDirectoryAsync(sourceDir, destDir, true, options, context);
            task.Wait();

            Console.ReadKey();
        }


        private static void FileSkippedCallback(object sender, TransferEventArgs e)
        {
            Console.WriteLine("Transfer skips. {0}.", e.Exception);
        }
    }
}
