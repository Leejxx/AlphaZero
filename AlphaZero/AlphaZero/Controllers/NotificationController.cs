using AlphaZero.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlphaZero.Controllers
{
    public class NotificationController : Controller
    {

        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: Notification
        [ChildActionOnly]
        public ActionResult GetReminders()
        {
            var reminders = db.reminds.ToList();
            return PartialView("_Notifications", reminders);
        }

        // GET: User
        public ActionResult Index()
        {
            return View(db.reminds.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reminds reminds = db.reminds.Find(id);
            if (reminds == null)
            {
                return HttpNotFound();
            }
            return View(reminds);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            reminds reminds = db.reminds.Find(id);
            return View(reminds);
        }

        [HttpPost]
        public ActionResult Edit(reminds reminds)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing record based on the reminder_id
                reminds existingReminder = db.reminds.SingleOrDefault(i => i.reminder_id == reminds.reminder_id);

                if (existingReminder != null)
                {
                    existingReminder.reminder_title = reminds.reminder_title;
                    existingReminder.reminder_desc = reminds.reminder_desc;
                    existingReminder.reminder_date = reminds.reminder_date;

                    // Update the attributes only if landlord_id and tenant_id are null
                    if (reminds.landlord_id == null)
                    {
                        existingReminder.landlord_id = null;
                    }
                    else
                    {
                        existingReminder.landlord_id = reminds.landlord_id;
                    }

                    if (reminds.tenant_id == null)
                    {
                        existingReminder.tenant_id = null;
                    }
                    else
                    {
                        existingReminder.tenant_id = reminds.tenant_id;
                    }

                    db.Entry(existingReminder).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }

            return View(reminds);
        }



        [HttpPost]
        public ActionResult Create(reminds reminds)
        {
            if (ModelState.IsValid)
            {
                reminds.reminder_status = 0;
                reminds.tenant_id = null;
                reminds.landlord_id = null;

                db.reminds.Add(reminds);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reminds);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            reminds reminds = db.reminds.Find(id);
            return View(reminds);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            reminds reminds = db.reminds.Find(id);
            db.reminds.Remove(reminds);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE reminds");
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}