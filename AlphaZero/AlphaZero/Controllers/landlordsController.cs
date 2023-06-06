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
    public class landlordsController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: landlords
        public ActionResult Index()
        {
            return View(db.landlords.ToList());
        }

        // GET: landlords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            landlord landlord = db.landlords.Find(id);
            if (landlord == null)
            {
                return HttpNotFound();
            }
            return View(landlord);
        }

        // GET: landlords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: landlords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "landlord_id,landlord_name,landlord_phoneNo,landlord_fee,landlord_due")] landlord landlord)
        {
            if (ModelState.IsValid)
            {
                db.landlords.Add(landlord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(landlord);
        }

        // GET: landlords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            landlord landlord = db.landlords.Find(id);
            if (landlord == null)
            {
                return HttpNotFound();
            }
            return View(landlord);
        }

        // POST: landlords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "landlord_id,landlord_name,landlord_phoneNo,landlord_fee,landlord_due")] landlord landlord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(landlord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(landlord);
        }

        // GET: landlords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            landlord landlord = db.landlords.Find(id);
            if (landlord == null)
            {
                return HttpNotFound();
            }
            return View(landlord);
        }

        // POST: landlords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            landlord landlord = db.landlords.Find(id);
            db.landlords.Remove(landlord);
            db.SaveChanges();
            return RedirectToAction("Index");
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
