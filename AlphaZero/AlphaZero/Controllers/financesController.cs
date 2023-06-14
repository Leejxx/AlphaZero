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
    public class financesController : Controller
    {

  
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: finances


        public ActionResult Index(string sortOrder)
        {
            ViewBag.FlowtypeSortParam = String.IsNullOrEmpty(sortOrder) ? "flowtype_desc" : "";
            ViewBag.InflowSortParam = sortOrder == "inflow" ? "inflow_desc" : "inflow";
            ViewBag.OutflowSortParam = sortOrder == "outflow" ? "outflow_desc" : "outflow";
            ViewBag.NotesSortParam = sortOrder == "notes" ? "notes_desc" : "notes";
            ViewBag.PaymentMethodSortParam = sortOrder == "paymentmethod" ? "paymentmethod_desc" : "paymentmethod";
            ViewBag.PaymentSortParam = sortOrder == "payment" ? "payment_desc" : "payment";
            ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            var finances = db.finances.Include(f => f.floor).Include(f => f.user);

            switch (sortOrder)
            {
                case "flowtype_desc":
                    finances = finances.OrderByDescending(f => f.finance_flowtype);
                    break;
                case "inflow":
                    finances = finances.OrderBy(f => f.finance_inflow);
                    break;
                case "inflow_desc":
                    finances = finances.OrderByDescending(f => f.finance_inflow);
                    break;
                case "outflow":
                    finances = finances.OrderBy(f => f.finance_outflow);
                    break;
                case "outflow_desc":
                    finances = finances.OrderByDescending(f => f.finance_outflow);
                    break;
                case "notes":
                    finances = finances.OrderBy(f => f.finance_desc);
                    break;
                case "notes_desc":
                    finances = finances.OrderByDescending(f => f.finance_desc);
                    break;
                case "paymentmethod":
                    finances = finances.OrderBy(f => f.finance_pMethod);
                    break;
                case "paymentmethod_desc":
                    finances = finances.OrderByDescending(f => f.finance_pMethod);
                    break;
                case "payment":
                    finances = finances.OrderBy(f => f.finance_type);
                    break;
                case "payment_desc":
                    finances = finances.OrderByDescending(f => f.finance_type);
                    break;
                case "date":
                    finances = finances.OrderBy(f => f.finance_date);
                    break;
                case "date_desc":
                    finances = finances.OrderByDescending(f => f.finance_date);
                    break;
                default:
                    finances = finances.OrderBy(f => f.finance_flowtype);
                    break;
            }


            return View(finances.ToList());
        }




        // GET: finances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            finance finance = db.finances.Find(id);
            if (finance == null)
            {
                return HttpNotFound();
            }
            return View(finance);
        }

        // GET: finances/Create
        public ActionResult Create()
        {
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id");
            ViewBag.user_id = new SelectList(db.users, "user_id", "user_password");
            return View();
        }

        // POST: finances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "finance_id,floor_id,finance_transaction,finance_desc,finance_pMethod,finance_type,finance_date,user_id,finance_inflow,finance_outflow,finance_flowtype")] finance finance, HttpPostedFileBase ReceiptFile)
        {
            if (ModelState.IsValid)
            {
                if (ReceiptFile != null && ReceiptFile.ContentLength > 0)
                {
                    // Save the uploaded file
                    string fileName = Path.GetFileName(ReceiptFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/assets/vendors/images/Receipts/"), fileName);
                    ReceiptFile.SaveAs(path);

                    finance.finance_receipt = fileName;
                }
                else
                {
                    finance.finance_receipt = null; // Set the finance_receipt to null if no file is uploaded
                }

                db.finances.Add(finance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", finance.floor_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "user_password", finance.user_id);
            return View(finance);
        }


        // GET: finances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            finance finance = db.finances.Find(id);
            if (finance == null)
            {
                return HttpNotFound();
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", finance.floor_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "user_password", finance.user_id);
            return View(finance);
        }

        // POST: finances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "finance_id,floor_id,finance_transaction,finance_desc,finance_pMethod,finance_type,finance_date,finance_receipt,user_id")] finance finance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", finance.floor_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "user_password", finance.user_id);
            return View(finance);
        }

        // GET: finances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            finance finance = db.finances.Find(id);
            if (finance == null)
            {
                return HttpNotFound();
            }
            return View(finance);
        }

        // POST: finances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            finance finance = db.finances.Find(id);
            db.finances.Remove(finance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SelectList GetPaymentOptions()
        {
            var paymentOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Full Payment", Value = "FullPayment" },
                new SelectListItem { Text = "Partial Payment", Value = "PartialPayment" }
            };

            return new SelectList(paymentOptions, "Value", "Text");
        }

        public ActionResult GetFile(string floorLayoutFileName)
        {
            string filePath = Server.MapPath("~/Content/assets/vendors/images/Receipts/" + floorLayoutFileName);
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