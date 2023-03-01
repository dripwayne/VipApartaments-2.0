﻿using VipApartaments.Data;
using VipApartaments.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml.Linq;
using System.Globalization;

namespace VipApartaments.Controllers
{
    public class Panel : Controller
    {
        private readonly AppDbContext db;


        public Panel(AppDbContext context)
        {
            db = context;
        }
        public class detailsModel
        {


            public int Id { get; set; }

            public string RoomType { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public int ToPay { get; set; }
            public int ToPayEUR { get; set; }
            public bool Pay { get; set; }


        }



        public class alldetailsModel
        {


            public int Id { get; set; }

            public string Email { get; set; }
            public string RoomType { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public int ToPay { get; set; }
            public bool Pay { get; set; }


        }
        public IActionResult Apanel(string Message)
        {
            string current_admin = (string)HttpContext.Session.GetString("adminUser");
            if (string.IsNullOrEmpty(current_admin))
            {
                return RedirectToAction("Clients", "Admin", new { Message = "Zalogij się jako administrator aby korzystać z panelu " });
            }
            ViewData["Message"] = Message;
            using (var cotex = db.Database.BeginTransaction())
            {

                var bookDetails = (from Booking in db.Booking
                                   join Details in db.Details
                                   on Booking.Id equals Details.Id
                                   join Rooms in db.Rooms on Booking.IdRoom equals Rooms.Id
                                   join Users in db.Clients on Booking.IdClient equals Users.Id

                                   select new
                                   {
                                       Users.Email,
                                       Booking.Id,
                                       Rooms.RoomType,
                                       Details.DateFrom,
                                       Details.DateTo,
                                       Booking.ToPay,
                                       Booking.Pay



                                   }).ToList();

                List<alldetailsModel> detailsList = new List<alldetailsModel>();
                foreach (var x in bookDetails)
                {
                    detailsList.Add(new alldetailsModel { Email = x.Email, Id = x.Id, RoomType = x.RoomType, DateFrom = x.DateFrom, DateTo = x.DateTo, ToPay = x.ToPay, Pay = x.Pay });
                }
                return View(detailsList);
              
            }
        }
        [HttpPost]
        public IActionResult Apanel(int Id, string RoomType, DateTime DateFrom, DateTime DateTo, string ToPay, bool Pay)
        {
            using (var contex = db.Database.BeginTransaction())
            {
                var booking = db.Booking.Include(b => b.Details).FirstOrDefault(b => b.Id == Id);
                var details = db.Details.Where(d => d.IdBook == booking.Id).FirstOrDefault();//*booking.Details.FirstOrDefault(d => d.IdBook==Id)*/
                int price = int.Parse(ToPay);
                int r_type = int.Parse(RoomType);
                booking.IdRoom = r_type;
                details.DateFrom = DateFrom;
                details.DateTo = DateTo;
                booking.ToPay = price;
                booking.Pay = Pay;

                db.SaveChanges();
                contex.Commit();



                return RedirectToAction("Apanel", "Panel", new { Message = "Dane uaktualnione" });



            }
        }

        public async Task<decimal> GetEuroExchangeRate()
        {
            using (var client = new HttpClient())
            {
                string url = "http://www.nbp.pl/kursy/xml/lasta.xml"; // adres do pliku XML z kursami walut
                var response = await client.GetAsync(url);
                var xmlString = await response.Content.ReadAsStringAsync();
                var xml = XDocument.Parse(xmlString);
                var euroRate = (from rate in xml.Descendants("pozycja")
                                where (string)rate.Element("kod_waluty") == "EUR"
                                select rate.Element("kurs_sredni").Value).FirstOrDefault();
                return decimal.Parse(euroRate.Replace(',', '.'), CultureInfo.InvariantCulture);
            }
        }
        public async Task<IActionResult> UpanelAsync()

        {

            //List<detailsModel> detailsList = GetBookingDetailsList();
            //return View(detailsList);
            int current_user = (int)HttpContext.Session.GetInt32("SessionID");
            //using (var cotex = db.Database.BeginTransaction())
            //{
                var bookDetails = (from Booking in db.Booking
                                   join Details in db.Details
                                   on Booking.Id equals Details.Id
                                   join Rooms in db.Rooms on Booking.IdRoom equals Rooms.Id
                                   join Users in db.Clients on Booking.IdClient equals Users.Id
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
            decimal euroExchangeRate = await GetEuroExchangeRate();
            foreach (var x in bookDetails)
            {
                int toPayEUR = (int)(x.ToPay / euroExchangeRate);
                detailsList.Add(new detailsModel { Id = x.Id, RoomType = x.RoomType, DateFrom = x.DateFrom, DateTo = x.DateTo, ToPay = x.ToPay,  ToPayEUR = toPayEUR, Pay = x.Pay });
            }
            return View(detailsList);

            

        }




    }
}
