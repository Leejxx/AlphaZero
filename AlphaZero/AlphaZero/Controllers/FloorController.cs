using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;

namespace AlphaZero.Controllers
{
    public class FloorController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: Floor
        public ActionResult Index()
        {
            var floors = db.floors.Include(f => f.landlord);
            return View(floors.ToList());
        }

        // GET: Floor/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            floor floor = db.floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }
            return View(floor);
        }

        // GET: Floor/Create
        public ActionResult Create()
        {
            ViewBag.landlord_id = new SelectList(db.landlords, "landlord_id", "landlord_name");
            return View();
        }

        // POST: Floor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "floor_id,floor_modemIP,floor_cctvQr,landlord_id")] floor floor, HttpPostedFileBase floorLayoutFile, HttpPostedFileBase floorCctvQrFile)
        {
            if (ModelState.IsValid)
            {
                if (db.floors.Any(x => x.floor_id == floor.floor_id))
                {
                    ViewBag.Message = "Floor ID already exists";
                    ViewBag.landlord_id = new SelectList(db.landlords, "landlord_id", "landlord_name", floor.landlord_id);
                    return View(floor);
                }

                if (floorCctvQrFile != null && floorCctvQrFile.ContentLength > 0)
                {
                    string cctvQrFileName = Path.GetFileName(floorCctvQrFile.FileName);
                    string cctvQrFilePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), cctvQrFileName);
                    floorCctvQrFile.SaveAs(cctvQrFilePath);
                    floor.floor_cctvQr = cctvQrFileName; // Save the original file name in the database
                }

                if (floorLayoutFile != null && floorLayoutFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(floorLayoutFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images"), fileName);
                    floorLayoutFile.SaveAs(filePath);
                    floor.floor_layout = fileName; // Save the unique file name in the database
                }

                db.floors.Add(floor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.landlord_id = new SelectList(db.landlords, "landlord_id", "landlord_name", floor.landlord_id);
            return View(floor);
        }


        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            floor floor = db.floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }

            ViewBag.landlord_id = new SelectList(db.landlords, "landlord_id", "landlord_name", floor.landlord_id);
            return View(floor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, floor updatedFloor, HttpPostedFileBase floorLayoutFile, HttpPostedFileBase floorCctvQrFile)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            floor floor = db.floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (floorLayoutFile != null && floorLayoutFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(floorLayoutFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images/"), fileName);
                    floorLayoutFile.SaveAs(filePath);
                    floor.floor_layout = fileName;
                }

                if (floorCctvQrFile != null && floorCctvQrFile.ContentLength > 0)
                {
                    string cctvQrFileName = Path.GetFileName(floorCctvQrFile.FileName);
                    string cctvQrFilePath = Path.Combine(Server.MapPath("~/Content/assets/vendors/images/"), cctvQrFileName);
                    floorCctvQrFile.SaveAs(cctvQrFilePath);
                    floor.floor_cctvQr = cctvQrFileName;
                }

                // Update other fields if needed
                floor.floor_id = updatedFloor.floor_id;
                floor.floor_modemIP = updatedFloor.floor_modemIP;
                floor.landlord_id = updatedFloor.landlord_id;

                db.Entry(floor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.landlord_id = new SelectList(db.landlords, "landlord_id", "landlord_name", floor.landlord_id);
            return View(floor);
        }


        // GET: Floor/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            floor floor = db.floors.Find(id);
            if (floor == null)
            {
                return HttpNotFound();
            }
            return View(floor);
        }

        // POST: Floor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            floor floor = db.floors.Find(id);
            db.floors.Remove(floor);
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


