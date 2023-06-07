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
		private readonly db_roomrentalEntities db = new db_roomrentalEntities();

		// GET: Login
		public ActionResult Index()
		{
			return View();
		}

<<<<<<< Updated upstream
        [HttpPost]
        [ValidateAntiForgeryToken]
=======
		[HttpPost]
		[ValidateAntiForgeryToken]
>>>>>>> Stashed changes
		public ActionResult Index(user objchk)
		{
			if (ModelState.IsValid)
			{
<<<<<<< Updated upstream
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
=======
				var obj = db.users.FirstOrDefault(a => a.user_name == objchk.user_name && a.user_password == objchk.user_password);

				if (obj != null)
				{
					Session["UserID"] = obj.user_id.ToString();
					Session["UserName"] = obj.user_name.ToString();

					return RedirectToAction("Index", "Home");
				}
				else
				{
					TempData["InvalidCredentials"] = "Invalid username or password";
				}
			}
>>>>>>> Stashed changes

			return View(objchk);
		}

		public ActionResult Logout()
		{
			Session.Clear();
			return RedirectToAction("Index", "Login");
		}
	}
}
