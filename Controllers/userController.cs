using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class UserController : Controller
	{
		public userTable userTbl = new userTable();

		[HttpPost]
		public ActionResult SignUp(userTable Users)
		{
			var result = userTbl.insertUser(Users);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult SignUp()
		{
			return View(userTbl);
		}

		public IActionResult Login()
		{
			return View();
		}
	}
}
