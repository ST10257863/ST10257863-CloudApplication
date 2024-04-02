using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class userController : Controller
	{
		public userTable userTbl = new userTable();
		[HttpPost]
		public IActionResult About(userTable Users)
		{
			var result = userTbl.insertUser(Users);
			return RedirectToAction("Index", "Home");
		}
		[HttpGet]
		//public async Task<IActionResult> About()
		//{
		//	View(userTbl);
		//}
		public IActionResult About()
		{
			return View(userTbl);
		}
	}
}
