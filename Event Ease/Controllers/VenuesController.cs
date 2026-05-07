using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Event_Ease.Data;
using Event_Ease.Models;
using Event_Ease.Services;

namespace Event_Ease.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        // The IBlobStorageService field allows the controller to interact with cloud-based 
        // storage via an abstraction, keeping the code modular (Microsoft, 2026a).
        private readonly IBlobStorageService _blobService;

        // Constructor Injection is used here to fulfill the Dependency Injection design pattern, 
        // allowing the framework to manage service lifetimes (Microsoft, 2026b).
        public VenuesController(ApplicationDbContext context, IBlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            // ViewData is used to maintain the state of the UI during filtering operations (Microsoft, 2026c).
            ViewData["CurrentFilter"] = searchString;

            var venues = from v in _context.Venues select v;

            if (!string.IsNullOrEmpty(searchString))
            {
                // LINQ (Language Integrated Query) provides a secure way to build conditional 
                // database queries based on user search criteria (Microsoft, 2026d).
                venues = venues.Where(v => v.Name.Contains(searchString) || v.Description.Contains(searchString));
            }

            return View(await venues.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.Id == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents Cross-Site Request Forgery (CSRF) (Microsoft, 2026e).
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Capacity,ImageFile")] Venue venue)
        {
            // ModelState.IsValid ensures that data sent by the user meets the requirements 
            // defined in the Venue model metadata (Microsoft, 2026f).
            if (ModelState.IsValid)
            {
                // Handle physical file upload: the IFormFile is processed by the service and 
                // the resulting URL string is stored in the database (Microsoft, 2026g).
                if (venue.ImageFile != null)
                {
                    venue.ImageUrl = await _blobService.UploadImageAsync(venue.ImageFile);
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Capacity,ImageUrl,ImageFile")] Venue venue)
        {
            if (id != venue.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Update Logic: The old blob is removed to prevent storage clutter 
                    // before a new file is uploaded (Microsoft, 2026a).
                    if (venue.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(venue.ImageUrl))
                        {
                            await _blobService.DeleteImageAsync(venue.ImageUrl);
                        }
                        venue.ImageUrl = await _blobService.UploadImageAsync(venue.ImageFile);
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.Id == id);
            if (venue == null) return NotFound();

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            // Constraint Check: Logic verifies that the venue is not linked to any 
            // bookings before allowing deletion, ensuring database integrity (Microsoft, 2026h).
            bool hasActiveBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            if (hasActiveBookings)
            {
                ViewBag.ErrorMessage = "Cannot delete this Venue because it has active bookings attached to it.";
                return View(venue);
            }

            // Storage cleanup: Ensures that the physical image file is deleted from 
            // the Azurite emulator upon record removal.
            if (!string.IsNullOrEmpty(venue.ImageUrl))
            {
                await _blobService.DeleteImageAsync(venue.ImageUrl);
            }
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.Id == id);
        }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Azure Blob Storage client library for .NET. [Online]
//Available at: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
//[Accessed 7 May 2026].

//Microsoft, 2026b.Dependency injection in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
//[Accessed 7 May 2026].

//Microsoft, 2026c.Views and ViewData in ASP.NET Core MVC. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview
//[Accessed 7 May 2026].

//Microsoft, 2026d.LINQ Queries in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/querying/
//[Accessed 7 May 2026].

//Microsoft, 2026e.Prevent Cross - Site Request Forgery(XSRF/CSRF) attacks. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery
//[Accessed 7 May 2026].

//Microsoft, 2026f.Model validation in ASP.NET Core MVC. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
//[Accessed 7 May 2026].

//Microsoft, 2026g.File uploads in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
//[Accessed 7 May 2026].

//Microsoft, 2026h.Relationship mapping in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
//[Accessed 7 May 2026].