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
		public IActionResult Index()
		{
			// Retrieve the userID from session
			var userID = HttpContext.Session.GetInt32("UserID");
			ViewData["userID"] = userID;


			// Retrieve all products from the database
			List<ProductModel> products = ProductModel.retrieveProducts();

			// Pass products to the view
			ViewData["Products"] = products;

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

		public IActionResult Transaction()
		{
			// Retrieve the userID from session
			int? userID = HttpContext.Session.GetInt32("UserID");
			ViewData["userID"] = userID;


			List<transactionModel> transactions = transactionModel.RetrieveUserTransactions(userID);
			ViewData["Transactions"] = transactions;

			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult TESTVIEW()
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
