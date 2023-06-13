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
    public class profitsController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: profits
        public ActionResult Index()
        {
            var profits = db.profits.Include(p => p.Investor);
            return View(profits.ToList());
        }

        // GET: profits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            return View(profit);
        }

        // GET: profits/Create
        public ActionResult Create()
        {
            ViewBag.Investor_id = new SelectList(db.Investors, "Investor_id", "investor_name");
            return View();
        }

        // POST: profits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "profit_id,Investor_id,profit_month,profit_value")] profit profit)
        {
            if (ModelState.IsValid)
            {
                db.profits.Add(profit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Investor_id = new SelectList(db.Investors, "Investor_id", "investor_name", profit.Investor_id);
            return View(profit);
        }

        // GET: profits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Investor_id = new SelectList(db.Investors, "Investor_id", "investor_name", profit.Investor_id);
            return View(profit);
        }

        // POST: profits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "profit_id,Investor_id,profit_month,profit_value")] profit profit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Investor_id = new SelectList(db.Investors, "Investor_id", "investor_name", profit.Investor_id);
            return View(profit);
        }

        // GET: profits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            profit profit = db.profits.Find(id);
            if (profit == null)
            {
                return HttpNotFound();
            }
            return View(profit);
        }

        // POST: profits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            profit profit = db.profits.Find(id);
            db.profits.Remove(profit);
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
