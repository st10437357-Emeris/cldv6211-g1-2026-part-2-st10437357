using Microsoft.AspNetCore.Http.HttpResults;

namespace Event_Ease.Models
{
    // This model is a specialized Data Transfer Object (DTO) designed to convey 
    // diagnostic information to the user during application failures (Microsoft, 2026a).
    public class ErrorViewModel
    {
        // RequestId captures the unique Trace Identifier from the HTTP context, 
        // allowing developers to find the specific error in server logs (Microsoft, 2026b).
        public string? RequestId { get; set; }

        // ShowRequestId is a read-only helper property that uses a lambda expression 
        // to determine if the ID should be rendered in the HTML view (Microsoft, 2026c).
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Handle errors in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling
//[Accessed 7 May 2026].

//Microsoft, 2026b.Logging in .NET Core and ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/
//[Accessed 7 May 2026].

//Microsoft, 2026c.C# Lambda expressions. [Online]
//Available at: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
//[Accessed 7 May 2026].