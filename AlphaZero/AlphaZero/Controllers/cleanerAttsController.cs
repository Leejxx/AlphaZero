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
    public class cleanerAttsController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: cleanerAtts
        public ActionResult Index(string floorLevel)
        {
            var floorLevels = db.floors.Select(f => new SelectListItem
            {
                Value = f.floor_id,
                Text = f.floor_id
            }).ToList();

            ViewBag.FloorLevels = new SelectList(floorLevels, "Value", "Text", floorLevel);

            var cleanerAtts = db.cleanerAtts.Include(c => c.floor).AsQueryable();

            if (!string.IsNullOrEmpty(floorLevel) && floorLevel != "All")
            {
                cleanerAtts = cleanerAtts.Where(c => c.floor.floor_id == floorLevel);
            }

            return View(cleanerAtts.ToList());
        }


        // GET: cleanerAtts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cleanerAtt cleanerAtt = db.cleanerAtts.Find(id);
            if (cleanerAtt == null)
            {
                return HttpNotFound();
            }
            return View(cleanerAtt);
        }

        // GET: cleanerAtts/Create
        public ActionResult Create()
        {
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr");
            return View();
        }

        // POST: cleanerAtts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cleanerAtt_id,floor_id,cleaner_date")] cleanerAtt cleanerAtt)
        {
            if (ModelState.IsValid)
            {
                db.cleanerAtts.Add(cleanerAtt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", cleanerAtt.floor_id);
            return View(cleanerAtt);
        }

        // GET: cleanerAtts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cleanerAtt cleanerAtt = db.cleanerAtts.Find(id);
            if (cleanerAtt == null)
            {
                return HttpNotFound();
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", cleanerAtt.floor_id);
            return View(cleanerAtt);
        }

        // POST: cleanerAtts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cleanerAtt_id,floor_id,cleaner_date")] cleanerAtt cleanerAtt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleanerAtt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", cleanerAtt.floor_id);
            return View(cleanerAtt);
        }

        // GET: cleanerAtts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cleanerAtt cleanerAtt = db.cleanerAtts.Find(id);
            if (cleanerAtt == null)
            {
                return HttpNotFound();
            }
            return View(cleanerAtt);
        }

        // POST: cleanerAtts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cleanerAtt cleanerAtt = db.cleanerAtts.Find(id);
            db.cleanerAtts.Remove(cleanerAtt);
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
