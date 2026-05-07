using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Event_Ease.Services
{
    // The BlobStorageService class implements the IBlobStorageService interface to handle 
    // binary data persistence in a cloud-native environment (Microsoft, 2026a).
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        // The constructor retrieves configuration settings from appsettings.json, enabling the 
        // service to connect to the local Azurite emulator for cloud emulation (Microsoft, 2026b).
        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureBlobStorage");
            _containerName = configuration["BlobSettings:ContainerName"];
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            // BlobContainerClient is utilized to manage the lifecycle of the storage container, 
            // ensuring it exists before an upload is attempted (Microsoft, 2026c).
            var containerClient = new BlobContainerClient(_connectionString, _containerName);

            // PublicAccessType.Blob is set to allow the generated URLs to be rendered 
            // in standard HTML <img> tags across the application.
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // A Globally Unique Identifier (GUID) is prepended to the file name to prevent 
            // naming collisions within the same storage container (Microsoft, 2026d).
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            // The IFormFile stream is opened and uploaded asynchronously, ensuring the 
            // application remains responsive during large file transfers (Microsoft, 2026e).
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            // The service returns the final URI, which is then stored in the SQL 
            // database for future reference.
            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                // The absolute URI is parsed to extract the specific blob name (file name) 
                // required to target the file for deletion (Microsoft, 2026c).
                var uri = new Uri(imageUrl);
                string fileName = Path.GetFileName(uri.LocalPath);

                var containerClient = new BlobContainerClient(_connectionString, _containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                // DeleteIfExistsAsync ensures that the service does not throw an exception 
                // if the file was already manually removed from storage.
                await blobClient.DeleteIfExistsAsync();
            }
            catch
            {
                // Exception handling ensures that storage cleanup failures do not 
                // interrupt the primary database transaction.
            }
        }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Azure Blob Storage client library for .NET. [Online]
//Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
//[Accessed 7 May 2026].

//Microsoft, 2026b.Use the Azurite emulator for local Azure Storage development. [Online]
//Available at: https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite
//[Accessed 7 May 2026].

//Microsoft, 2026c.BlobContainerClient Class(Azure.Storage.Blobs). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/azure.storage.blobs.blobcontainerclient
//[Accessed 7 May 2026].

//Microsoft, 2026d.Guid.NewGuid Method(System). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid
//[Accessed 7 May 2026].

//Microsoft, 2026e.File uploads in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
//[Accessed 7 May 2026].