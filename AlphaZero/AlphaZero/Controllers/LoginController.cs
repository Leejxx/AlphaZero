using AlphaZero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;

namespace AlphaZero.Controllers
{
    public class LoginController : Controller
    {

        db_roomrentalEntities db = new db_roomrentalEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Index(user objchk)
		{
			if (ModelState.IsValid)
			{
				using (db_roomrentalEntities db = new db_roomrentalEntities()) ;
				{
					var obj = db.users.Where(a => a.user_name.Equals(objchk.user_name) && a.user_password.Equals(objchk.user_password)).FirstOrDefault();

					if (obj != null)
					{
						Session["UserID"] = obj.user_id.ToString();
						Session["UserName"] = obj.user_name.ToString();

						return RedirectToAction("Index", "Home");
					}

					else
					{
						ModelState.AddModelError("", "The Username or Password Incorrect");


					}
				}
			}




			return View();
		}
		public ActionResult Logout()
		{
			Session.Clear();
			return RedirectToAction("Index", "Login");
		}

	}


}
