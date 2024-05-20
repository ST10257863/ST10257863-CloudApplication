using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CloudApplication.Models;
using Newtonsoft.Json; // Import Newtonsoft.Json for JSON serialization

namespace CloudApplication.Controllers
{
	public class CartController : Controller
	{
		private readonly CartModel cartModel = new CartModel(new HttpContextAccessor());

		// Private method to check if user is logged in
		private int? GetLoggedInUserId()
		{
			var userID = HttpContext.Session.GetInt32("UserID");
			if (userID == null)
			{
				TempData["RedirectReason"] = "You need to log in to perform this action.";
				return null;
			}
			return userID;
		}

		public IActionResult Cart()
		{

			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}
			ViewData["userID"] = userID;
			var cartItems = cartModel.GetCart((int)userID);
			return View(cartItems); // Pass the list of cart items directly to the view
		}

		[HttpPost]
		public IActionResult AddToCart(int productID, int quantity)
		{
			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}

			cartModel.AddToCart((int)userID, productID, quantity);
			TempData["SuccessMessage"] = "Item successfully added to cart.";

			// Set the success message in ViewData and return the same view
			ViewData["SuccessMessage"] = TempData["SuccessMessage"];
			return RedirectToAction("Index", "Home");
		}


		[HttpPost]
		public IActionResult UpdateCart(int productID, int quantity)
		{
			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}

			cartModel.UpdateCart((int)userID, productID, quantity);
			return RedirectToAction("Cart");
		}

		[HttpPost]
		public IActionResult RemoveFromCart(int productID)
		{
			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}

			cartModel.RemoveFromCart((int)userID, productID);
			return RedirectToAction("Cart");
		}

		[HttpPost]
		public IActionResult ClearCart()
		{
			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}

			cartModel.ClearCart((int)userID);
			return RedirectToAction("Cart");
		}

		[HttpPost]
		public IActionResult CheckOut()
		{
			var userID = GetLoggedInUserId();
			if (userID == null)
			{
				return RedirectToAction("Login", "User");
			}

			cartModel.CheckOut((int)userID);
			TempData["SuccessMessage"] = "Order placed successfully.";
			return RedirectToAction("Index", "Home");
		}
	}
}
