using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
			// Retrieve the userID from session
			var userID = HttpContext.Session.GetInt32("UserID");
			ViewData["userID"] = userID;

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

		public IActionResult MyWork()
		{
			return View();
		}

		//public IActionResult Cart()
		//{
		//	return View();
		//}

		public IActionResult Transaction()
		{
			// Retrieve the userID from session
			int? userID = HttpContext.Session.GetInt32("UserID");
			ViewData["userID"] = userID;

			List<TransactionModel> transactions = TransactionModel.RetrieveUserTransactions(userID);
			ViewData["Transactions"] = transactions;

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
