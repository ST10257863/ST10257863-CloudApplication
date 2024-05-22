using CloudApplication.Models;
using Microsoft.AspNetCore.Http;
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
		public ActionResult Login(string password, string name)
		{
			var userModel = new userModel();
			int userID = userModel.SelectUser(password, name);
			if (userID != -1)
			{
				// User found, store userID in session
				HttpContext.Session.SetInt32("UserID", userID);

				// Redirect to home page
				return RedirectToAction("Index", "Home");
			}
			else
			{
				// User not found, show error message
				TempData["ErrorMessage"] = "Invalid password or name. Please try again.";
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

		public IActionResult LogOut()
		{
			// Clear the UserID session variable
			HttpContext.Session.Remove("UserID");

			// Redirect to the home page
			return RedirectToAction("Index", "Home");

		}
	}
}
