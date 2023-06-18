using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using AlphaZero.Models;
using System.Text;

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
                    finances = finances.OrderBy(f => f.finance_date);
                    break;
            }

            return View(finances.ToList());
        }

        public ActionResult Summary()
        {
            var financeData = db.finances
                .GroupBy(t => new { t.finance_date.Year, t.finance_date.Month, t.floor_id })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    FloorId = g.Key.floor_id,
                   Profit = g.Sum(t => t.finance_inflow - t.finance_outflow)

                })
                .OrderBy(t => t.FloorId)
                .ToList();

            var profitByFloor = new Dictionary<int, Dictionary<int, Dictionary<string, double>>>();

            var floorNames = db.floors.ToDictionary(f => f.floor_id, f => f.floor_id);

            foreach (var financeEntry in financeData)
            {
                var year = financeEntry.Year;
                var month = financeEntry.Month;
                var floorName = !string.IsNullOrEmpty(financeEntry.FloorId) ? financeEntry.FloorId : "General";

                var profit = financeEntry.Profit ?? 0.0;


                if (!profitByFloor.ContainsKey(year))
                {
                    profitByFloor[year] = new Dictionary<int, Dictionary<string, double>>();
                }

                if (!profitByFloor[year].ContainsKey(month))
                {
                    profitByFloor[year][month] = new Dictionary<string, double>();
                }

                profitByFloor[year][month][floorName] = profit;
            }

            return View(profitByFloor);
        }

        public void CalculateCurrentMonthProfit()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            //var currentDate = DateTime.Now.AddMonths(-1);
            //var currentMonth = currentDate.Month;
            //var currentYear = currentDate.Year;

            // Retrieve the investors from the tb_investor table
            var investors = db.Investors.ToList();

            var financeData = db.finances
                .Where(t => t.finance_date.Month == currentMonth && t.finance_date.Year == currentYear && t.floor_id != null)
                .GroupBy(t => new { t.floor_id })
                .Select(g => new
                {
                    FloorId = g.Key.floor_id,
                    Profit = g.Sum(t => t.finance_inflow - t.finance_outflow)
                })
                .ToList();

            // Calculate the total profit for the current month
            var profit = financeData.Sum(t => t.Profit);
            var partner = 3;
            var lot = db.Investors.Select(x => x.investor_lotNo).Distinct().Count();
            var priceperlot = 50000;
            profit = profit * 0.6 / (lot);
            var p_month = $"{currentYear}-{currentMonth}";
           

            foreach (var investor in investors)
            {
                var sharing = profit * (investor.investor_holdvalue / priceperlot);

                var parsedMonth = currentMonth; // Use the current month directly

                var existingProfit = db.profits.FirstOrDefault(p => p.Investor_id == investor.Investor_id && p.profit_month.Month == parsedMonth);


                if (existingProfit != null)
                {
                    // Update the existing profit entry
                    existingProfit.profit_value = (double)sharing;
                    db.Entry(existingProfit).State = EntityState.Modified;

                    var user = db.users.Find(investor.user_id);
                }
                else
                {
                    // Create a new profit entry

                    var profitEntry = new profit
                    {
                        Investor_id = investor.Investor_id,
                        profit_month = new DateTime(currentYear, currentMonth, 1),
                        profit_value = (double)sharing
                    };

                    // Add the profit entry to the table
                    db.profits.Add(profitEntry);

                 

                }
            }

            // Save the changes to the database
            db.SaveChanges();
        }


        // GET: finances/ExportToCSV
        public ActionResult ExportToCSV()
        {
            var finances = db.finances.Include(f => f.floor).Include(f => f.user).ToList();

            var csvContent = "Finance ID,Floor ID,Flowtype,Inflow,Outflow,Description,PaymentMethod,Receipt,Date,UserID\n";

            foreach (var finance in finances)
            {
                csvContent += $"{finance.finance_id},{finance.floor_id},{finance.finance_flowtype},{finance.finance_inflow}, {finance.finance_outflow},{finance.finance_desc},{finance.finance_pMethod},{finance.finance_receipt},{finance.finance_date},{finance.user_id}\n";
            }

            var bytes = Encoding.ASCII.GetBytes(csvContent);
            var result = new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "finances.csv"
            };

            return result;
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

                // Set the finance_inflow and finance_outflow to 0 if their respective fields are disabled
                if (Request.Form["finance_inflow"] == null)
                {
                    finance.finance_inflow = 0;
                }
                if (Request.Form["finance_outflow"] == null)
                {
                    finance.finance_outflow = 0;
                }

                db.finances.Add(finance);
                db.SaveChanges();
                CalculateCurrentMonthProfit();
                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_cctvQr", finance.floor_id);
            ViewBag.user_id = new SelectList(db.users, "user_id", "user_password", finance.user_id);
            return View(finance);
        }


        // GET: Finances/Edit/5
        public ActionResult Edit(int id)
        {
            var finance = db.finances.Find(id);
            if (finance == null)
            {
                return HttpNotFound();
            }

            return View(finance);
        }

        // POST: Finances/UpdateReceipt
        [HttpPost]
        public ActionResult UpdateReceipt(int financeId, HttpPostedFileBase receiptFile)
        {
            if (receiptFile != null && receiptFile.ContentLength > 0)
            {
                // Save the new receipt file to the appropriate location
                var fileName = Path.GetFileName(receiptFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/assets/vendors/images/Receipts/"), fileName);
                receiptFile.SaveAs(path);

                // Update the finance record with the new receipt file name
                var finance = db.finances.Find(financeId);
                finance.finance_receipt = fileName;
                db.SaveChanges();
            }

            // Redirect back to the Edit page
            return RedirectToAction("Edit", new { id = financeId });
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
                CalculateCurrentMonthProfit();
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
            CalculateCurrentMonthProfit();
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