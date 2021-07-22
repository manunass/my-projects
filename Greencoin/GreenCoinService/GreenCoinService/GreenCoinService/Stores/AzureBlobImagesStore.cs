using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;
using System.IO;
using GreenCoinService.Stores.Interfaces;
using GreenCoinService.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace GreenCoinService.Stores
{
    public class AzureBlobImagesStore : IImagesStore
    {
        private readonly CloudBlobContainer _cloudBlobContainer; 
  
        public AzureBlobImagesStore(IOptions<AzureStorageSettings> options)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            CloudBlobClient cloudblobclient = storageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = cloudblobclient.GetContainerReference("images"); 
        }

        public async Task Delete(string folderName, string imageId)
        {
            var userDirectory = _cloudBlobContainer.GetDirectoryReference(folderName);
            var cloudblockblob = userDirectory.GetBlockBlobReference(imageId);
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

        public async Task DeleteFolder(string folderName)
        {
            var userDirectory = _cloudBlobContainer.GetDirectoryReference(folderName);
            try
            {
                var userBlobs = await userDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.Metadata, null, null, null, null);

                foreach (IListBlobItem blob in userBlobs.Results)
                {
                    if (blob.GetType() == typeof(CloudBlob) || blob.GetType().BaseType == typeof(CloudBlob))
                    {
                        await ((CloudBlob)blob).DeleteIfExistsAsync();
                    }
                }
            }
            catch (StorageException e)
            {
                throw new StorageErrorException($"Could not delete all images in {folderName}", e, e.RequestInformation.HttpStatusCode);
            }
        }

        public async Task<List<string>> ListUris(string folderName)
        {
            var userDirectory = _cloudBlobContainer.GetDirectoryReference(folderName);
            try
            {
                var userBlobs = await userDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.Metadata, null, null, null, null);
                var imagesUris = userBlobs.Results.Select(a => a.StorageUri.PrimaryUri.AbsoluteUri).ToList();
                return imagesUris;
            }
            catch(StorageException e)
            {
                throw new StorageErrorException("Could not download block list from Azure Blob Storage", e.RequestInformation.HttpStatusCode);
            }
        }

        public async Task<string> Upload(string folderName, byte[] bytes, string contentType)
        {
            string imageId = Guid.NewGuid().ToString();
            var userDirectory = _cloudBlobContainer.GetDirectoryReference(folderName);
            var cloudBlockBlob = userDirectory.GetBlockBlobReference(imageId);
            cloudBlockBlob.Properties.ContentType = contentType;
            try
            {
                using var stream = new MemoryStream(bytes, writable: false);

                await cloudBlockBlob.UploadFromStreamAsync(stream);
                return imageId;
            }
            catch (StorageException e)
            {
                throw new StorageErrorException("Could not write to Azure Blob Storage", e, e.RequestInformation.HttpStatusCode);
            }
        }
    }
}
