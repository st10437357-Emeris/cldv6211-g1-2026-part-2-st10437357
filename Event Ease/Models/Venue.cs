using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_Ease.Models
{
    // The Venue class acts as a domain entity, mapping directly to a table in the 
    // SQL database via Entity Framework Core (Microsoft, 2026a).
    public class Venue
    {
        // The Id property is the Primary Key, utilized by the framework to maintain 
        // referential integrity across the system (Microsoft, 2026b).
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        // ImageUrl stores the persistent string path to the file hosted in the 
        // Azurite blob storage emulator (Microsoft, 2026c).
        public string? ImageUrl { get; set; }

        // The [NotMapped] attribute prevents Entity Framework from creating a column for 
        // IFormFile, as it is used only for the transport of binary data during 
        // the request lifecycle (Microsoft, 2026a).
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // The ICollection navigation property defines a One-to-Many relationship, 
        // indicating that a single venue can be associated with multiple booking 
        // records (Microsoft, 2026d).
        public ICollection<Booking>? Bookings { get; set; }
    }
}

//REFERNCE LIST:
//Microsoft, 2026a. NotMappedAttribute Class (System.ComponentModel.DataAnnotations.Schema). [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.schema.notmappedattribute
//[Accessed 7 May 2026].

//Microsoft, 2026b.Modeling Keys - EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/keys
//[Accessed 7 May 2026].

//Microsoft, 2026c.Azure Blob Storage client library for .NET. [Online]
//Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
//[Accessed 7 May 2026].

//Microsoft, 2026d.Relationship mapping in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
//[Accessed 7 May 2026].