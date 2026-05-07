using Azure.Core;
using Event_Ease.Data;
using Event_Ease.Models;
using Event_Ease.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Ease.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // The IBlobStorageService is defined as a private field to enable the use of 
        // external cloud storage emulation within the controller logic (Microsoft, 2026a).
        private readonly IBlobStorageService _blobService;

        // Constructor injection is used to provide the controller with the database context 
        // and blob service, adhering to the Dependency Injection principle (Microsoft, 2026b).
        public EventsController(ApplicationDbContext context, IBlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            // LINQ (Language Integrated Query) is used here to create a flexible query 
            // against the database that only executes when ToListAsync is called (Microsoft, 2026c).
            var events = from e in _context.Events select e;

            if (!string.IsNullOrEmpty(searchString))
            {
                events = events.Where(e => e.Name.Contains(searchString) || e.Description.Contains(searchString));
            }

            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Security measure to prevent Cross-Site Request Forgery (CSRF) (Microsoft, 2026d).
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile")] Event @event)
        {
            if (ModelState.IsValid)
            {
                // If a file is present, the controller delegates the binary upload to the 
                // BlobStorageService and stores the resulting URI in the model (Microsoft, 2026e).
                if (@event.ImageFile != null)
                {
                    @event.ImageUrl = await _blobService.UploadImageAsync(@event.ImageFile);
                }

                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,ImageFile")] Event @event)
        {
            if (id != @event.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Image management logic ensures that the old file is deleted from 
                    // storage before the new one is uploaded to save space (Microsoft, 2026e).
                    if (@event.ImageFile != null)
                    {
                        if (!string.IsNullOrEmpty(@event.ImageUrl))
                        {
                            await _blobService.DeleteImageAsync(@event.ImageUrl);
                        }
                        @event.ImageUrl = await _blobService.UploadImageAsync(@event.ImageFile);
                    }

                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null) return NotFound();

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            // Data Integrity: This check prevents a foreign key violation by ensuring no 
            // active bookings exist before allowing deletion (Microsoft, 2026f).
            bool hasActiveBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);
            if (hasActiveBookings)
            {
                ViewBag.ErrorMessage = "Cannot delete this Event because it has active bookings attached to it.";
                return View(@event);
            }

            // Cleanup: Deleting the image from blob storage ensures no 'orphaned' 
            // files remain in the cloud after the database record is gone.
            if (!string.IsNullOrEmpty(@event.ImageUrl))
            {
                await _blobService.DeleteImageAsync(@event.ImageUrl);
            }
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
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

//Microsoft, 2026c.LINQ Queries in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/querying/
//[Accessed 7 May 2026].

//Microsoft, 2026d.Prevent Cross - Site Request Forgery(XSRF/CSRF) attacks. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery
//[Accessed 7 May 2026].

//Microsoft, 2026e.File uploads in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
//[Accessed 7 May 2026].

//Microsoft, 2026f.Relationship mapping in EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
//[Accessed 7 May 2026].