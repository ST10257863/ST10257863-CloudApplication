using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

public class TransactionController : Controller
{
	[HttpPost]
	public IActionResult PlaceOrder(int productID, int quantity, int newTransactionGroupID)
	{
		int? userID = HttpContext.Session.GetInt32("UserID");
		if (userID == null)
		{
			TempData["RedirectReason"] = "You need to log in to place an order.";
			return RedirectToAction("Login", "User");
		}
		var transactionModel = new TransactionModel();
		var result = transactionModel.PlaceOrder(userID, productID, quantity, newTransactionGroupID);

		if (result > 0)
		{
			// Order placed successfully
			TempData["SuccessMessage"] = "Order placed successfully.";
		}
		else
		{
			// Handle failure
			TempData["ErrorMessage"] = "Failed to place order. Please try again.";
		}

		return RedirectToAction("Index", "Home");
	}


	[HttpPost]
	public IActionResult RetrieveUserTransactions(int? userID)
	{
		var transactionModel = new TransactionModel();
		var transactions = transactionModel.RetrieveUserTransactions(userID).OrderBy(t => t.TransactionGroupID).ToList();
		return View("Transaction", transactions);
	}
}
