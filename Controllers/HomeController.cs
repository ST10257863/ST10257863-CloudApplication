using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
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

		public IActionResult Index(string sortOrder)
		{
			// Set the sort order based on the current state
			string currentSortOrder = sortOrder ?? "default";
			string nextSortOrder;

			if (currentSortOrder == "price_asc")
			{
				nextSortOrder = "price_desc";
			}
			else if (currentSortOrder == "price_desc")
			{
				nextSortOrder = "default";
			}
			else
			{
				nextSortOrder = "price_asc";
			}

			// Retrieve all products from the database
			List<ProductModel> products = ProductModel.RetrieveProducts(currentSortOrder);

			// Pass products and the next sort order to the view
			ViewData["Products"] = products;
			ViewData["CurrentSortOrder"] = currentSortOrder;
			ViewData["NextSortOrder"] = nextSortOrder;

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

		[HttpGet]
		public IActionResult MyWork()
		{
			//var productsWithImages = ProductModel.RetrieveProductsImages();
			//return View(productsWithImages);
			return View();
		}

		public IActionResult Transaction()
		{
			// Retrieve the userID from session
			var userID = HttpContext.Session.GetInt32("UserID");
			if (userID == null)
			{
				TempData["RedirectReason"] = "You need to log in to perform this action.";
				return RedirectToAction("Login", "User");
			}

			// Retrieve user transactions
			var transactionController = new TransactionController();
			return transactionController.RetrieveUserTransactions(userID);
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
