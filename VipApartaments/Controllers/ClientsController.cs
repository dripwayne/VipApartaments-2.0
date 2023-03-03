using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VipApartaments.Data;
using VipApartaments.Models;

namespace VipApartaments.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext db;
      

        public ClientsController(AppDbContext context)
        {
            db = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await db.Clients.ToListAsync());
        }
        public IActionResult Admin(string Message)
        {
            ViewData["Message"] = Message;
            return View();
        }
        public IActionResult Login(string Message)
        {
            ViewData["Message"] = Message;
            return View();
        }
        public bool IsValidPhoneNumber(string phonenumberString)
        {
            int number;
            bool isInteger = int.TryParse(phonenumberString, out number);

            bool has9Digits = phonenumberString.Length == 9;

            return isInteger && has9Digits;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            bool isMatch = Regex.IsMatch(email, pattern);

            return isMatch;
        }

        // GET: Clients/Create

        public IActionResult Create()
        {

            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Phone,Email,password")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                db.Add(clients);
                await db.SaveChangesAsync();



                return RedirectToAction("Login", "Clients");
            }
            return View();
        }

        [HttpPost]
       public IActionResult Login(string email, string password)
        { 
          using (var contex = db.Database.BeginTransaction())
            {
                var any = db.Clients.Where(c => c.Email == email && c.password == password).FirstOrDefault() ;
                if (any==null)
                {
                    ViewData["Message"] = "Nieprawidłowy login lub hasło";
                    return View();
                }
               
                var id_finder = from c in db.Clients where c.Email == email select c;
                var id_checker = id_finder.FirstOrDefault<Clients>();
                HttpContext.Session.SetString("userName", id_checker.FirstName);
                HttpContext.Session.SetInt32("SessionID", id_checker.Id);
                HttpContext.Session.SetString("SessionEmail", id_checker.Email);
                ViewData["Message"] = "Zalogowano";
                return RedirectToAction("Book", "Booking");

                //HttpContext.Session.SetString("Session_Username", username);
                
            }
            
        }
        [HttpPost]
        public IActionResult Admin(string UserName, string Pass)
        {
            using (var contex = db.Database.BeginTransaction())
            {
                var x = db.Users.Where(c => c.UserName == UserName && c.Pass == Pass).FirstOrDefault();
                if (x == null)
                {
                    ViewData["Message"] = "Nieprawidłowy login lub hasło";
                    return View();
                }
                var id_finder = from c in db.Users where c.UserName == UserName select c;
                var id_checker = id_finder.FirstOrDefault<User>();
                HttpContext.Session.SetString("adminUser", id_checker.UserName);
                return RedirectToAction("Apanel", "Panel");

            }
        }
        public IActionResult LogOut()
        { 
                HttpContext.Session.Clear();
            
            return RedirectToAction("Login", "Clients", new { Message = "Wylogowano" });
        
            
        }
    }
}
