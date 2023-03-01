using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VipApartaments.Data;
using Microsoft.AspNetCore.Http;
using VipApartaments.Models;
using System.Text.RegularExpressions;

namespace VipApartaments.Controllers
{
   
    
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

      

        public BookingController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Book(string Message)

        {
            ViewData["Message"] = Message;
            var room = _context.Rooms.ToList();

            return View(room);
        }
        
       
        
            [HttpPost]
            public IActionResult Book(DateTime DateFrom, DateTime DateTo, string select_type)
        {
            if (DateFrom == DateTime.MinValue || DateTo == DateTime.MinValue || DateFrom > DateTo || DateFrom < DateTime.Today)
            {
                return RedirectToAction("Book", "Booking", new { Message  = "Wybierz poprawny zakres dat" });
            }
            else if ( select_type != null)
            {
                int select_t = int.Parse(select_type);
               
           
                int current_user = (int)HttpContext.Session.GetInt32("SessionID");
                int r_price = _context.Rooms.Where(x => x.Id == select_t).Select(x => x.RoomPrice).First();
                TimeSpan timeSpan = DateTo - DateFrom;
                int totalDays = timeSpan.Days;
                int totalPrice = r_price * totalDays;
                using (var contex = _context.Database.BeginTransaction())
                {
                    var insertBook = (new Booking { IdClient = current_user, IdRoom = select_t, IdMethodOfPayment = 2, ToPay = totalPrice, Pay = false, });
                    _context.Booking.Add(insertBook);
                    _context.SaveChanges();
                   
                    var insertedBooking = _context.Booking.First(b => b.Id == insertBook.Id);

                    var insertDetails = new Details { IdBook = insertedBooking.Id, DateFrom = DateFrom, DateTo = DateTo };
                    _context.Details.Add(insertDetails);
                    _context.SaveChanges();

                    contex.Commit();
                    
                    return RedirectToAction("Upanel", "Panel");
                    //return RedirectToAction("Upanel", "Panel"/*, new { Message = "Dane dodane" }*/);



                }
                
            }

            ViewData["Message"] = "nie działa";
            return View();
        }

       

        // GET: Booking

        //public async Task<IActionResult> Index()
        //{
        //    return View();
        //   //var appDbContext = _context.Booking.Include(b => b.IdClientNavigation).Include(b => b.IdMethodOfPaymentNavigation).Include(b => b.IdRoomNavigation).Include(b=>b.Details);
        //    return View(await appDbContext.ToListAsync());
        //}

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.IdClientNavigation)
                .Include(b => b.IdMethodOfPaymentNavigation)
                .Include(b => b.IdRoomNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["IdMethodOfPayment"] = new SelectList(_context.Payment, "Id", "PaymentMethod");
            ViewData["IdRoom"] = new SelectList(_context.Rooms, "Id", "RoomType");
            return View();
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdClient,IdRoom,IdMethodOfPayment,ToPay,Pay")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Email", booking.IdClient);
            ViewData["IdMethodOfPayment"] = new SelectList(_context.Payment, "Id", "PaymentMethod", booking.IdMethodOfPayment);
            ViewData["IdRoom"] = new SelectList(_context.Rooms, "Id", "RoomType", booking.IdRoom);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Email", booking.IdClient);
            ViewData["IdMethodOfPayment"] = new SelectList(_context.Payment, "Id", "PaymentMethod", booking.IdMethodOfPayment);
            ViewData["IdRoom"] = new SelectList(_context.Rooms, "Id", "RoomType", booking.IdRoom);
            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdClient,IdRoom,IdMethodOfPayment,ToPay,Pay")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdClient"] = new SelectList(_context.Clients, "Id", "Email", booking.IdClient);
            ViewData["IdMethodOfPayment"] = new SelectList(_context.Payment, "Id", "PaymentMethod", booking.IdMethodOfPayment);
            ViewData["IdRoom"] = new SelectList(_context.Rooms, "Id", "RoomType", booking.IdRoom);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.IdClientNavigation)
                .Include(b => b.IdMethodOfPaymentNavigation)
                .Include(b => b.IdRoomNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Booking == null)
            {
                return Problem("Entity set 'AppDbContext.Booking'  is null.");
            }
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return _context.Booking.Any(e => e.Id == id);
        }
    }
}
