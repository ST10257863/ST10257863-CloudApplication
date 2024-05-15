using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class LoginController : Controller
	{
		private readonly LoginModel login;

		public LoginController()
		{
			login = new LoginModel();
		}

		[HttpPost]
		public ActionResult Login(string email, string name)
		{
			var loginModel = new LoginModel();
			int userID = loginModel.SelectUser(email, name);
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
	}
}
