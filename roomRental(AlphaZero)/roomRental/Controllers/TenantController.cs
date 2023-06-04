using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using roomRental.Models;

namespace roomRental.Controllers
{
    public class TenantController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: Tenant
        public ActionResult Index()
        {
            var tenants = db.tenants.Include(t => t.room);
            return View(tenants.ToList());
        }

        // GET: Tenant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tenant tenant = db.tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenant/Create
        public ActionResult Create()
        {
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_status");
            return View();
        }

        // POST: Tenant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenant_id,tenant_ic,tenant_uploadIC,tenant_name,tenant_contract,tenant_phoneNo,tenant_emergencyNo,tenant_noSiri,tenant_inDate,tenant_outDate,tenant_outTime,tenant_outstanding,tenant_paymentStatus,room_id")] tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_status", tenant.room_id);
            return View(tenant);
        }

        // GET: Tenant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tenant tenant = db.tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_status", tenant.room_id);
            return View(tenant);
        }

        // POST: Tenant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tenant_id,tenant_ic,tenant_uploadIC,tenant_name,tenant_contract,tenant_phoneNo,tenant_emergencyNo,tenant_noSiri,tenant_inDate,tenant_outDate,tenant_outTime,tenant_outstanding,tenant_paymentStatus,room_id")] tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_status", tenant.room_id);
            return View(tenant);
        }

        // GET: Tenant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tenant tenant = db.tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tenant tenant = db.tenants.Find(id);
            db.tenants.Remove(tenant);
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
