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
        

            if (Session["UserType"] != null && Session["UserType"].Equals("2"))
            {
                var userId = Convert.ToInt32(Session["UserID"]);
                var tb_investor = db.Investors.FirstOrDefault(i => i.user_id == userId);

                if (tb_investor != null)
                {
                    int i_id = tb_investor.Investor_id;
                    var currentMonth = DateTime.Now.Month;
                    var currentYear = DateTime.Now.Year;
                    var month = $"{currentYear}-{currentMonth}";

                    var tb_profit = db.profits
                        .Where(p => p.Investor_id == i_id && !p.profit_month.Equals(month))
                        .ToList();

                    if (tb_profit != null)
                    {
                        foreach (var profitEntry in tb_profit)
                        {
                            // Retrieve investor name from tb_user
                            var investor = db.users.FirstOrDefault(u => u.user_id == tb_investor.user_id);
                            if (investor != null)
                                profitEntry.InvestorUsername = investor.user_name;
                        }

                        tb_profit = tb_profit
                            .OrderByDescending(p => p.profit_month) // Order by year
                           
                            .ToList();

                        return View(tb_profit);
                    }
                    else
                    {
                        return View();
                    }
                }
            }

            var profit = db.profits
                .Include(t => t.Investor)
                .ToList()
                .Select(p => new profit
                {
                    profit_id = p.profit_id,
                    Investor_id = p.Investor_id,
                    profit_month = p.profit_month,
                    profit_value = p.profit_value,
                    p_lot = p.Investor.investor_lotNo,
                    InvestorUsername = db.users.FirstOrDefault(u => u.user_id == p.Investor.user_id)?.user_name
                })
                     .OrderByDescending(p => p.profit_month) // Order by year

                .ToList();


            return View(profit);

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