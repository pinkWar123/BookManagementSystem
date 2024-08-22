using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BookManagementSystem.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace BookManagementSystem.Application.Services
{
    public class AzureBlobService
    {
        BlobServiceClient _blobClient;
        BlobContainerClient _containerClient;
        private readonly AzureConfig _azureConfig;
        
        public AzureBlobService(IOptions<AzureConfig> azureConfig)
        {
            _azureConfig = azureConfig.Value;
            _blobClient = new BlobServiceClient(_azureConfig.ConnectionStrings);
            _containerClient = _blobClient.GetBlobContainerClient(_azureConfig.BlobContainer);
        }

        public async Task<List<string>> UploadFiles(List<IFormFile> files)
        {

            var azureResponse = new List<Azure.Response<BlobContentInfo>>();
            var fileList = new List<string>();
            foreach(var file in files)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                // Get the file extension
                string fileExtension = Path.GetExtension(file.FileName);
                // Create a new file name with a timestamp
                string newFileName = $"{fileNameWithoutExtension}_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}{fileExtension}";

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    var client = await _containerClient.UploadBlobAsync(newFileName, memoryStream, default);
                    azureResponse.Add(client);
                }

                fileList.Add(newFileName);
            };

            return fileList.Select(file => $"https://miniieltsbypinkwar.blob.core.windows.net/apiimages/{file}").ToList();
        }

        public async Task<List<BlobItem>> GetUploadedBlobs()
        {
            var items = new List<BlobItem>();
            var uploadedFiles = _containerClient.GetBlobsAsync();
            await foreach (BlobItem file in uploadedFiles)
            {
                items.Add(file);
            }

            return items;
        }
    }
}