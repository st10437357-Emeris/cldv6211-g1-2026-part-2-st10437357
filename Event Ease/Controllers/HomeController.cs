using Event_Ease.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace Event_Ease.Controllers
{
    // The HomeController inherits from the base Controller class, providing the 
    // fundamental plumbing required to return MVC Views (Microsoft, 2026a).
    public class HomeController : Controller
    {
        // ILogger is injected here to allow the application to record system events, 
        // which is critical for debugging and monitoring health (Microsoft, 2026b).
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // The Index action serves as the application's root landing page, 
        // processing the default route defined in Program.cs (Microsoft, 2026c).
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // The Error action handles application-wide exceptions. The ResponseCache 
        // attribute ensures that error pages are never cached, providing users 
        // with the most accurate diagnostic ID for troubleshooting (Microsoft, 2026d).
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // ErrorViewModel is utilized to capture the unique Trace Identifier, 
            // linking the user's view to the server's internal logs (Microsoft, 2026b).
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//Reference LIST:
//Microsoft, 2026a. Controllers and Actions in ASP.NET Core MVC. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
//[Accessed 7 May 2026].

//Microsoft, 2026b.Logging in .NET Core and ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/
//[Accessed 7 May 2026].

//Microsoft, 2026c.Routing to controller actions in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing
//[Accessed 7 May 2026].

//Microsoft, 2026d.Response caching in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response
//[Accessed 7 May 2026].