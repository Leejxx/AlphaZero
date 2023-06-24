using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AlphaZero.Models;

namespace AlphaZero.Controllers
{
    public class cleanerAttsController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: cleanerAtts
        public ActionResult Index(string floorLevel, int? selectedMonth)
        {
            // Get the current month and year
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int currentYear = currentDate.Year;

            // Generate month dropdown items
            ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(i => new { Value = i, Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }), "Value", "Text", selectedMonth);
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.FloorLevel = floorLevel;

            // Generate floor dropdown items
            var floorLevels = db.floors.Select(f => new SelectListItem
            {
                Value = f.floor_id,
                Text = f.floor_id,
                Selected = f.floor_id == floorLevel
            }).ToList();

            // Add "All" option at the beginning
            floorLevels.Insert(0, new SelectListItem
            {
                Value = "All",
                Text = "All",
                Selected = floorLevel == "All"
            });

            ViewBag.FloorLevels = new SelectList(floorLevels, "Value", "Text");


            // Get all cleanerAtts
            var cleanerAtts = db.cleanerAtts.Include(c => c.floor).ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(floorLevel) && floorLevel != "All")
            {
                cleanerAtts = cleanerAtts.Where(c => c.floor.floor_id == floorLevel).ToList();
            }

            if (selectedMonth.HasValue)
            {
                int selectedMonthValue = selectedMonth.Value;
                cleanerAtts = cleanerAtts.Where(c => c.cleaner_date.Month == selectedMonthValue).ToList();
            }

            ViewBag.FloorLevels = new SelectList(floorLevels, "Value", "Text");

            // Calculate the total salary
            float totalSalary = (float)cleanerAtts.Sum(c => c.cleaner_salary);

            // Store the total salary in ViewBag
            ViewBag.TotalSalary = totalSalary;

            return View(cleanerAtts);
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
        public ActionResult Create(string floor_id, DateTime? cleaner_date, decimal? cleaner_salary)
        {
            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", floor_id);

            // Use the preset values if provided, otherwise use the current month and year
            int currentMonth = cleaner_date?.Month ?? DateTime.Now.Month;
            int currentYear = cleaner_date?.Year ?? DateTime.Now.Year;

            ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(i => new { Value = i, Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) }), "Value", "Text", currentMonth);

            // Create a new cleanerAtt object and assign the preset values
            cleanerAtt newCleanerAtt = new cleanerAtt
            {
                floor_id = floor_id,
                cleaner_date = cleaner_date ?? DateTime.Now.Date,
                cleaner_salary = cleaner_salary != null ? Convert.ToDouble(cleaner_salary) : 35
            };

            return View(newCleanerAtt);
        }



        // GET: cleanerAtts/RecordTodayAttendance
        public ActionResult RecordTodayAttendance()
        {
            cleanerAtt newCleanerAtt = new cleanerAtt
            {
                cleaner_date = DateTime.Now.Date, // Set today's date (without time component)
                cleaner_salary = 35
            };

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id");
            ViewBag.ExceededAttendance = false; // Set default value

            // Pass the preset values as route parameters
            return RedirectToAction("Create", new { floor_id = newCleanerAtt.floor_id, cleaner_date = newCleanerAtt.cleaner_date, cleaner_salary = newCleanerAtt.cleaner_salary });
        }



        // POST: cleanerAtts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cleanerAtt_id,floor_id,cleaner_date,cleaner_salary")] cleanerAtt cleanerAtt, int currentMonth, int currentYear)
        {
            if (ModelState.IsValid)
            {
                // Check if there are existing records for the current month and floor
                var existingRecords = db.cleanerAtts
                    .Where(c => c.floor_id == cleanerAtt.floor_id && c.cleaner_date.Month == currentMonth && c.cleaner_date.Year == currentYear)
                    .ToList();

                // Set cleanerAtt_count to the next available count
                cleanerAtt.cleanerAtt_count = existingRecords.Count + 1;

                db.cleanerAtts.Add(cleanerAtt);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", cleanerAtt.floor_id);
            ViewBag.ExceededAttendance = false; // Set default value
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

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", cleanerAtt.floor_id);
            return View(cleanerAtt);
        }

        // POST: cleanerAtts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cleanerAtt_id,floor_id,cleaner_date,cleaner_salary")] cleanerAtt cleanerAtt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cleanerAtt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.floor_id = new SelectList(db.floors, "floor_id", "floor_id", cleanerAtt.floor_id);
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
        public ActionResult DeleteConfirmed(int id, int? selectedMonth, string floorLevel)
        {
            cleanerAtt cleanerAtt = db.cleanerAtts.Find(id);

            if (cleanerAtt == null)
            {
                return HttpNotFound();
            }

            // Delete the record
            db.cleanerAtts.Remove(cleanerAtt);
            db.SaveChanges();

            // Recalculate CleanerAttendanceCount based on floorLevel and selectedMonth filters
            var filteredCleanerAtts = db.cleanerAtts.Include(c => c.floor);

            if (!string.IsNullOrEmpty(floorLevel) && floorLevel != "All")
            {
                filteredCleanerAtts = filteredCleanerAtts.Where(c => c.floor.floor_id == floorLevel);
            }

            if (selectedMonth.HasValue)
            {
                int selectedMonthValue = selectedMonth.Value;
                filteredCleanerAtts = filteredCleanerAtts.Where(c => c.cleaner_date.Month == selectedMonthValue);
            }

            return RedirectToAction("Index", new { selectedMonth = selectedMonth, floorLevel = floorLevel });
        }

    }
}