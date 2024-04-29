using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CloudApplication.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		//Here is where we give the webpage the ability to access the the subpages. without this is will not be found
		public IActionResult Index(int userID)
		{
			return View();
		}

		public IActionResult AboutUs()
		{
			return View();
		}

		public IActionResult ContactUs()
		{
			return View();
		}

		public IActionResult MyWork()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
