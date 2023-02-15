using VipApartaments.Data;
using VipApartaments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace VipApartaments.Controllers
{
    public class Panel :Controller
    {
        private readonly AppDbContext db;
     

        public Panel(AppDbContext context)
        {
            db = context;
        }
        public class detailsModel
        {
            public int userID { get; set; }
          public string userName { get; set; }
          public int Id { get; set; }
            public string RoomType { get; set; }
           public DateTime DateFrom { get; set;}
            public DateTime DateTo { get; set; }
            public int ToPay { get; set; }
            public bool Pay { get; set; }
           
                                              
        }
        public IActionResult Upanel()

        {
            int current_user = (int)HttpContext.Session.GetInt32("SessionID");
            using ( var cotex = db.Database.BeginTransaction()) { 
            var details = (from Booking in db.Booking
                                           join Details in db.Details
                                           on Booking.Id equals Details.Id
                                           join Rooms in db.Rooms on Booking.IdRoom equals Rooms.Id
                                          
                                           where Booking.IdClient == current_user
                                           select new
                                           {
                                               Booking.Id,
                                              Rooms.RoomType,
                                              Details.DateFrom,
                                              Details.DateTo,
                                              Booking.ToPay,
                                              Booking.Pay

                                           }).ToList();

            List<detailsModel> detailsList = new List<detailsModel>();
            foreach (var x in details)
            {
                    detailsList.Add(new detailsModel { Id = x.Id, RoomType = x.RoomType, DateFrom=x.DateFrom, DateTo=x.DateTo, ToPay=x.ToPay,Pay= x.Pay });
            }
            return View(detailsList);
            }
        }
       
    }
}
