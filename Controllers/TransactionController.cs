using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class transactionController : Controller
	{
		[HttpPost]
		public IActionResult PlaceOrder(int productID, int quantity)
		{
			int? userID = HttpContext.Session.GetInt32("UserID");
			if (userID == null)
			{
				TempData["RedirectReason"] = "You need to log in to place an order.";
				return RedirectToAction("Login", "User");
			}


			var transactionModel = new transactionModel();
			var result = transactionModel.PlaceOrder(userID, productID, quantity);

			if (result > 0)
			{
				// Order placed successfully
				return RedirectToAction("IndexRetrieveProducts", "Product");
			}
			else
			{
				// Handle failure
				TempData["ErrorMessage"] = "Failed to place order. Please try again.";
				return RedirectToAction("IndexRetrieveProducts", "Product");
			}

		}


		[HttpPost]
		public IActionResult RetrieveUserTransactionsCon(int? userID)
		{
			var transactionModel = new transactionModel();
			var result = transactionModel.RetrieveUserTransactions(userID);
			return RedirectToAction();
		}
	}
}
