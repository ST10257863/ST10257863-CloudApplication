using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class userController : Controller
	{
		public userTable userTbl = new userTable();

		[HttpPost]
		public ActionResult About(userTable Users)
		{
			var result = userTbl.insertUser(Users);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult About()
		{
			return View(userTbl);
		}
	}
}
