using System;
using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Datacontracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;
using Aub.Eece503e.ChatService.Web.Store.Exceptions;
using System.IO;

namespace Aub.Eece503e.ChatService.Web.Store.Azure
{
    public class AzureBlobImageStore : IImageStore
    {
        private readonly CloudBlobContainer _cloudBlobContainer; 
  
        public AzureBlobImageStore(IOptions<AzureStorageSettings> options)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            CloudBlobClient cloudblobclient = storageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudblobclient.GetContainerReference("images"); 
        }

        public async Task Delete(string imageId)
        {
            CloudBlockBlob cloudblockblob = _cloudBlobContainer.GetBlockBlobReference(imageId);
            try
            { 
                if (!await cloudblockblob.ExistsAsync())
                {
                    throw new StorageErrorException($"Image with id {imageId} was not found", 404);
                }
                await cloudblockblob.DeleteAsync();
            }
            catch (StorageException e)
            {
                throw new StorageErrorException("Could not write to Azure Table", e, e.RequestInformation.HttpStatusCode);
            }

        }

        public async Task<byte[]> Download(string imageId)
        {
            CloudBlockBlob cloudblockblob = _cloudBlobContainer.GetBlockBlobReference(imageId);
            
            try
            {
                if(!await cloudblockblob.ExistsAsync())
                {
                    throw new StorageErrorException($"Image with id {imageId} was not found", 404);
                }
                using var stream = new MemoryStream();
                await cloudblockblob.DownloadToStreamAsync(stream); 
                return stream.ToArray();
            }
            catch (StorageException e)
            {
                throw new StorageErrorException("Could not write to Azure Table", e, e.RequestInformation.HttpStatusCode);
            }
        }

        public async Task<string> Upload(byte[] bytes)
        {
            string imageId = Guid.NewGuid().ToString() ; 
            CloudBlockBlob cloudblockblob = _cloudBlobContainer.GetBlockBlobReference(imageId);
            try
            {
                using var stream = new MemoryStream(bytes, writable: false);
                await cloudblockblob.UploadFromStreamAsync(stream);
                return imageId;
            }
            catch (StorageException e)
            {
                throw new StorageErrorException("Could not write to Azure Table", e, e.RequestInformation.HttpStatusCode);
            }
        }
    }
}
