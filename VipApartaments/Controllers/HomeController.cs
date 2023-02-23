using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VipApartaments.Data;
using VipApartaments.Models;

namespace VipApartaments.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string Message)
        {
            ViewData["Message"] = Message;

            //var current_user = HttpContext.Session.GetInt32("SessionID");
            //if(current_user == null)
            //{
            //    return RedirectToAction("Index", "Home", new { Message = "Najpierw się zaloguj" });
            //}
            //var instructors = _context.Users.ToList();
            //return View(instructors);
            return View();
        }

        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
