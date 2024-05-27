using CloudApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudApplication.Controllers
{
	public class CartController : Controller
	{
		private readonly CartModel cartModel = new CartModel();

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
			if (cartModel.CheckItemInCart(productID) == true)
			{
				// Get the current quantity of the item in the cart
				int currentQuantity = cartModel.GetQuantity((int)userID, productID);

				// Add the new quantity to the current quantity
				int newQuantity = currentQuantity + quantity;

				// Update the quantity of the item in the cart
				cartModel.UpdateCart((int)userID, productID, newQuantity);
				TempData["SuccessMessage"] = "Item quantity updated in cart.";
			}
			else
			{
				if (cartModel.AddToCart((int)userID, productID, quantity) > 0)
				{
					TempData["SuccessMessage"] = "Item successfully added to cart.";
				}
				else
				{
					TempData["ErrorMessage"] = "Item could not be added to cart, out of stock.";
				}
			}

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
