using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;

namespace AlphaZero.Controllers
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

        [HttpPost]
        public ActionResult GetRoomNumbers(string floorId)
        {
            var roomNumbers = db.rooms
             .Where(r => r.floor_id == floorId && r.room_status == "Available")
                .Select(r => new SelectListItem
                {
                    Value = r.room_id.ToString(),
                    Text = r.room_number.ToString()
                })
                .ToList();

            return Json(roomNumbers);
        }


        // GET: Tenant/Create
        public ActionResult Create()
        {
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_number");



            var uniqueFloorIds = db.rooms.Select(r => r.floor_id).Distinct().ToList();
            ViewBag.floor_id = new SelectList(uniqueFloorIds);
            return View();


        }

        // POST: Tenant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenant_id,tenant_ic,tenant_uploadIC,tenant_name,tenant_contract,tenant_phoneNo,tenant_emergencyNo,tenant_noSiri,tenant_inDate,tenant_outDate,tenant_outSession,tenant_outstanding,tenant_paymentStatus,room_id")] tenant tenant, HttpPostedFileBase tenantIC, HttpPostedFileBase tenantContract)
        {
            if (ModelState.IsValid)
            {
                if (tenantIC != null && tenantIC.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(tenantIC.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), fileName);
                    tenantIC.SaveAs(filePath);
                    tenant.tenant_uploadIC = fileName; // Save the unique file name in the database
                }

                if (tenantContract != null && tenantContract.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(tenantContract.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), fileName);
                    tenantContract.SaveAs(filePath);
                    tenant.tenant_contract = fileName; // Save the unique file name in the database
                }

                db.tenants.Add(tenant);
                db.SaveChanges();

                var room = db.rooms.FirstOrDefault(r => r.room_id == tenant.room_id);
                if (room != null)
                {
                    room.room_status = "Booked";
                    db.SaveChanges();
                }

                return RedirectToAction("Index");


            }

            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_id", tenant.room_id);
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

            var uniqueFloorIds = db.rooms.Select(r => r.floor_id).Distinct().ToList();
            ViewBag.floor_id = new SelectList(uniqueFloorIds);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_number", tenant.room_id);
            return View(tenant);
        }

        // POST: Tenant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, tenant updatedTenant, HttpPostedFileBase tenantIC, HttpPostedFileBase tenantContract)
        {
            

            tenant tenant = db.tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var previousRoom = db.rooms.FirstOrDefault(r => r.room_id == tenant.room_id);
                if (previousRoom != null)
                {
                    previousRoom.room_status = "Available";
                    db.SaveChanges();
                }


              

                if (tenantIC != null && tenantIC.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(tenantIC.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), fileName);
                    tenantIC.SaveAs(filePath);
                    tenant.tenant_uploadIC = fileName; // Save the unique file name in the database
                }
              

                if (tenantContract != null && tenantContract.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(tenantContract.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), fileName);
                    tenantContract.SaveAs(filePath);
                    tenant.tenant_contract = fileName; // Save the unique file name in the database
                }

                // Update other fields if needed
                tenant.tenant_id = updatedTenant.tenant_id;
              
                tenant.tenant_ic = updatedTenant.tenant_ic;
                tenant.tenant_name = updatedTenant.tenant_name;
               
                tenant.tenant_phoneNo = updatedTenant.tenant_phoneNo;
                tenant.tenant_emergencyNo = updatedTenant.tenant_emergencyNo;
                tenant.tenant_noSiri = updatedTenant.tenant_noSiri;
                tenant.tenant_inDate = updatedTenant.tenant_inDate;
                tenant.tenant_outDate = updatedTenant.tenant_outDate;
                tenant.tenant_outSession = updatedTenant.tenant_outSession;
                tenant.tenant_outstanding = updatedTenant.tenant_outstanding;
                tenant.tenant_paymentStatus = updatedTenant.tenant_paymentStatus;
                tenant.room_id = updatedTenant.room_id;
                var room = db.rooms.FirstOrDefault(r => r.room_id == tenant.room_id);
                if (room != null)
                {
                    room.room_status = "Booked";
                    db.SaveChanges();
                }
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Information updated successfully.";

                return RedirectToAction("Edit");
            }

            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_number", tenant.room_id);
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

            var room = db.rooms.FirstOrDefault(r => r.room_id == tenant.room_id);
            if (room != null)
            {
                room.room_status = "Available";
            }

            db.tenants.Remove(tenant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetFile(string floorLayoutFileName)
        {
            string filePath = Server.MapPath("~/Content/assets/vendors/images/" + floorLayoutFileName);
            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, "image/png"); // Adjust the content type according to the actual file type
            }
            else
            {
                return HttpNotFound();
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
