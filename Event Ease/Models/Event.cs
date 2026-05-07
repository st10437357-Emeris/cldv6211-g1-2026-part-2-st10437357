using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Ease.Models
{
    // This class serves as a domain model, representing the structure of an 'Event'
    // entity within the application's database (Microsoft, 2026a).
    public class Event
    {
        // The Id property is automatically recognized as the Primary Key by Entity Framework,
        // facilitating unique record identification (Microsoft, 2026b).
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        // ImageUrl stores a string reference to the file's location in Azure Blob Storage,
        // rather than the binary file data itself (Microsoft, 2026c).
        public string? ImageUrl { get; set; }

        // The [NotMapped] attribute is critical; it instructs Entity Framework to exclude 
        // this property from the SQL table as the database cannot store a raw IFormFile object (Microsoft, 2026a).
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. NotMappedAttribute Class (System.ComponentModel.DataAnnotations.Schema). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.schema.notmappedattribute
//[Accessed 7 May 2026].

//Microsoft, 2026b.Keys - EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/keys
//[Accessed 7 May 2026].

//Microsoft, 2026c.File uploads in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
//[Accessed 7 May 2026].