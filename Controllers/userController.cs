using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CloudApplication.Controllers
{
	public class UserController : Controller
	{
		public userModel userTbl = new userModel();

		//asp-action"Signup"
		[HttpPost]
		public ActionResult SignUp(userModel Users)
		{
			var result = userTbl.insertUser(Users);
			return RedirectToAction("Index", "Home");
		}
		//asp-action"Login"
		[HttpPost]
		public ActionResult Login(string email, string name)
		{
			//var loginModel = new LoginModel();
			//int userID = loginModel.SelectUser(email, name);
			var userModel = new userModel();
			int userID = userModel.SelectUser(email, name);
			if (userID != -1)
			{
				// User found, proceed with login logic (e.g., set authentication cookie)
				// For demonstration, redirecting to a dummy page
				return RedirectToAction("Index", "Home", new
				{
					userID = userID
				});
			}
			else
			{
				// User not found, handle accordingly (e.g., show error message)
				//return View("LoginFailed");
				TempData["ErrorMessage"] = "Invalid email or name. Please try again.";
				return RedirectToAction("Login", "User");

			}
		}

		//[HttpGet]
		public ActionResult SignUp()
		{
			return View(userTbl);
		}
		//[HttpGet]
		public ActionResult Login()
		{
			return View();
		}




	}
}
