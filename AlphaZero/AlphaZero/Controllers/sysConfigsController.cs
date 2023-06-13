using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;

namespace AlphaZero.Controllers
{
    public class sysConfigsController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: sysConfigs
        public ActionResult Index()
        {
            var sysConfigs = db.sysConfigs.ToList();
            return View(sysConfigs);
        }


        [HttpPost]
        public ActionResult UpdateConfig(string bankAcc, string bankName)
        {
            // Update the sysConfig values based on the submitted form values
            UpdateSysConfigValue("bankAcc", bankAcc);
            UpdateSysConfigValue("bankName", bankName);

            // Redirect back to the Index action
            return RedirectToAction("Index");
        }

        private void UpdateSysConfigValue(string category, string value)
        {
            var existingConfig = db.sysConfigs.SingleOrDefault(c => c.category == category);

            if (existingConfig != null)
            {
                existingConfig.value = value;
                db.SaveChanges();
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
