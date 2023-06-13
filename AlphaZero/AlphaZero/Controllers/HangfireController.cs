using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaZero.Controllers
{
    public class HangfireController : Controller
    {
        // GET: Hangfire
        public ActionResult Index()
        {
            return View();
        }

        // GET: Hangfire/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Hangfire/Create
        [HttpPost]
        public ActionResult Create()
        {
            // Schedule a recurring job to run every day at a specific time
            RecurringJob.AddOrUpdate(() => CreateReminderJob(), Cron.Daily);
            
            return RedirectToAction("Index");
        }

        private void CreateReminderJob()
        {
            Console.WriteLine("Hangfire job is firing!");
        }
    }
}
