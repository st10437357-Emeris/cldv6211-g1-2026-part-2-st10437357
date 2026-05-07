using Azure.Core;
using Event_Ease.Data;
using Event_Ease.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Event_Ease.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // The constructor implements Dependency Injection to provide access to the database context, 
        // ensuring the controller remains loosely coupled (Microsoft, 2026a).
        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            // Eager Loading via .Include() is used here to retrieve related Venue and Event data in 
            // a single query, preventing 'Lazy Loading' performance issues (Microsoft, 2026b).
            var bookings = _context.Bookings.Include(b => b.Event).Include(b => b.Venue).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // LINQ is utilized to filter the server-side data based on user input (Microsoft, 2026b).
                bookings = bookings.Where(b => b.Venue.Name.Contains(searchString) || b.Event.Name.Contains(searchString));
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            // SelectList objects are passed to the View via ViewData to populate HTML dropdown menus 
            // with Name strings instead of ID integers (Microsoft, 2026c).
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protects against Cross-Site Request Forgery (CSRF) attacks (Microsoft, 2026e).
        public async Task<IActionResult> Create([Bind("Id,VenueId,EventId,BookingDate")] Booking booking)
        {
            // The [Bind] attribute prevents 'overposting' by strictly limiting which model properties 
            // the user can update (Microsoft, 2026c).
            if (ModelState.IsValid)
            {
                // Custom validation logic using AnyAsync() ensures data integrity by preventing 
                // overlapping bookings in the database (Microsoft, 2026d).
                bool isAlreadyBooked = await _context.Bookings
                    .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate.Date == booking.BookingDate.Date);

                if (isAlreadyBooked)
                {
                    // ModelState.AddModelError stops the save and returns a UI-friendly error (Microsoft, 2026d).
                    ModelState.AddModelError("BookingDate", "Double Booking Error: This venue is already booked.");
                    ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                    ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                    return View(booking);
                }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VenueId,EventId,BookingDate")] Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Validation in the Edit method includes an ID check (b.Id != booking.Id) to ensure 
                    // the record doesn't conflict with its own current state (Microsoft, 2026d).
                    bool isAlreadyBooked = await _context.Bookings
                        .AnyAsync(b => b.VenueId == booking.VenueId
                                    && b.BookingDate.Date == booking.BookingDate.Date
                                    && b.Id != booking.Id);

                    if (isAlreadyBooked)
                    {
                        ModelState.AddModelError("BookingDate", "Double Booking Error: Already booked.");
                        ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", booking.EventId);
                        ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", booking.VenueId);
                        return View(booking);
                    }

                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // --- NEW DELETE METHODS ADDED HERE ---

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Include related data so the user can see what they are deleting
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- END OF NEW DELETE METHODS ---

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}

//REFERENCE LIST:
//Microsoft, 2026a. Dependency injection in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
//[Accessed 7 May 2026].

//Microsoft, 2026b.Loading Related Data - EF Core. [Online]
//Available at: https://learn.microsoft.com/en-us/ef/core/querying/related-data/
//[Accessed 7 May 2026].

//Microsoft, 2026c.Model Binding in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding
//[Accessed 7 May 2026].

//Microsoft, 2026d.Model validation in ASP.NET Core MVC. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
//[Accessed 7 May 2026].

//Microsoft, 2026e.Prevent Cross - Site Request Forgery(XSRF/CSRF) attacks in ASP.NET Core. [Online]
//Available at: https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery
//[Accessed 7 May 2026].