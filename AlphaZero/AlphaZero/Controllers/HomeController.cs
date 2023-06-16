using AlphaZero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaZero.Controllers
{
    public class HomeController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.user = db.users.Count();
            ViewBag.tenantcount = db.tenants.Count();
            ViewBag.landlords = db.landlords.Count();
            ViewBag.room = db.rooms.Count();
            ViewBag.availableRoom = db.rooms.Count(r => r.room_status == "Available");
            ViewBag.bookedRoom = db.rooms.Count(r => r.room_status == "Booked");


            var availableRooms = db.rooms.Where(r => r.room_status == "Available").ToList();

            // Pass the available rooms to the view
            return View(availableRooms);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}