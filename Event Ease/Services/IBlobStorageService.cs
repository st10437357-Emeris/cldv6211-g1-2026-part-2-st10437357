using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Event_Ease.Services
{
    // This interface defines the contract for blob storage operations, facilitating 
    // the 'Dependency Inversion Principle' by allowing the application to depend 
    // on abstractions rather than concrete implementations (Microsoft, 2026a).
    public interface IBlobStorageService
    {
        // UploadImageAsync defines the standard method for transmitting binary file 
        // data from the web server to a cloud-based storage container (Microsoft, 2026b).
        Task<string> UploadImageAsync(IFormFile file);

        // DeleteImageAsync provides the method signature for removing orphaned or 
        // outdated files from the storage service to ensure data consistency 
        // and storage efficiency (Microsoft, 2026b).
        Task DeleteImageAsync(string imageUrl);
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Dependency injection in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
//[Accessed 7 May 2026].

//Microsoft, 2026b.File uploads in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
//[Accessed 7 May 2026].