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
    public class InvestorController : Controller
    {
        private db_roomrentalEntities db = new db_roomrentalEntities();

        // GET: Investor
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        // GET: Investor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Investor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Investor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,user_password,user_name,user_email")] user user, [Bind(Include = "investor_id,investor_name,investor_lotNo,investor_holdvalue")] Investor investor)
        {
            if (ModelState.IsValid)
            {
                // Set the default user_type
                user.user_type = "2";

                // Save the user record
                db.users.Add(user);
                db.SaveChanges();

                // Retrieve the generated user_id
                int userId = user.user_id;

                // Assign the user_id to the investor
                investor.user_id = userId;

                // Save the investor record
                db.Investors.Add(investor);
                db.SaveChanges();

                // Call the SendCredentials method of EmailController to send the credentials
                EmailController emailController = new EmailController();
                emailController.SendCredentials(user.user_name, user.user_password, user.user_email, investor.investor_name);


                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Investor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = db.users.Include(u => u.Investors).SingleOrDefault(u => u.user_id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Ensure there is at least one investor associated with the user
            if (user.Investors.Count == 0)
            {
                // Create a new investor and associate it with the user
                Investor investor = new Investor();
                user.Investors.Add(investor);
            }

            return View(user);
        }


        // POST: Investor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_password,user_name,user_email")] user user, [Bind(Include = "investor_id,investor_name,investor_lotNo,investor_holdvalue")] Investor investor)
        {
            if (ModelState.IsValid)
            {
                // Set the default user_type
                user.user_type = "2";

                // Update the user record
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                // Retrieve the generated user_id
                int userId = user.user_id;

                // Assign the user_id to the investor
                investor.user_id = userId;

                // Retrieve the existing investor record based on the user_id
                Investor existingInvestor = db.Investors.SingleOrDefault(i => i.user_id == user.user_id);

                // Update the investor record
                if (existingInvestor != null)
                {
                    // Update the investor record
                    existingInvestor.investor_name = investor.investor_name;
                    existingInvestor.investor_lotNo = investor.investor_lotNo;
                    existingInvestor.investor_holdvalue = investor.investor_holdvalue;

                    db.Entry(existingInvestor).State = EntityState.Modified;
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Investor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user user = db.users.Include(u => u.Investors).SingleOrDefault(u => u.user_id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Ensure there is at least one investor associated with the user
            if (user.Investors.Count == 0)
            {
                // Create a new investor and associate it with the user
                Investor investor = new Investor();
                user.Investors.Add(investor);
            }

            return View(user);
        }

        // POST: Investor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);

            // Retrieve the existing investor record based on the user_id
            Investor existingInvestor = db.Investors.SingleOrDefault(i => i.user_id == user.user_id);
            db.Investors.Remove(existingInvestor);
            db.users.Remove(user);
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
