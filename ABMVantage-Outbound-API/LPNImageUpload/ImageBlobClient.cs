using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace ABMVantage_Outbound_API.LPRImageUpload
{
    public class ImageBlobClient : IImageBlobClient
    {
        private readonly IConfiguration _configuration;
        public ImageBlobClient(IConfiguration configuration)
        { 
            _configuration = configuration;
        }  
        public async Task UploadBlobAsync(string filename, Stream stream)
        {
            string? containerName = GetContainerName();

            var blobServiceClient = new BlobServiceClient(GetBlobStorageConnection());
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(filename);

            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = "image/jpeg";

            await blobClient.DeleteIfExistsAsync();

            stream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(stream, blobHttpHeader);
        }

        private string? GetBlobStorageConnection()
        {
            return _configuration.GetSection("BlobStorage").GetValue<string>("ConnectionString");
        }
        private string? GetContainerName()
        {
            return _configuration.GetSection("BlobStorage").GetValue<string>("ImageContainer");
        }
    }
}
