using System.Text.Json;

namespace CloudApplication.Models
{
	public class CartModel
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CartModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		private ISession GetSession()
		{
			return _httpContextAccessor.HttpContext.Session;
		}

		// GetCart method retrieves the cart items from session and deserializes them from JSON
		public List<CartItem> GetCart(int userID)
		{
			var session = GetSession();
			var cartItemsJson = session.GetString($"CartItems_{userID}");
			if (cartItemsJson == null)
			{
				return new List<CartItem>();
			}
			return DeserializeCart(cartItemsJson);
		}

		// SaveCart method serializes the cart items to JSON and saves them in session
		private void SaveCart(int userID, List<CartItem> cartItems)
		{
			var session = GetSession();
			var cartItemsJson = SerializeCart(cartItems);
			session.SetString($"CartItems_{userID}", cartItemsJson);
		}

		// Serialize cart items to JSON
		private string SerializeCart(List<CartItem> cartItems)
		{
			return JsonSerializer.Serialize(cartItems);
		}

		// Deserialize cart items from JSON
		private List<CartItem> DeserializeCart(string cartItemsJson)
		{
			return JsonSerializer.Deserialize<List<CartItem>>(cartItemsJson);
		}

		public void AddToCart(int userID, int productID, int quantity)
		{
			var product = ProductModel.RetrieveProductByID(productID);
			var cartItems = GetCart(userID);

			var existingItem = FindCartItem(cartItems, userID, productID);
			if (existingItem == null)
			{
				cartItems.Add(new CartItem
				{
					UserID = userID,
					ProductID = productID,
					ProductName = product.Name,
					Price = product.Price,
					Quantity = quantity
				});
			}
			else
			{
				existingItem.Quantity += quantity;
			}

			SaveCart(userID, cartItems);
		}

		// Find a cart item in the list based on userID and productID
		private CartItem FindCartItem(List<CartItem> cartItems, int userID, int productID)
		{
			foreach (var item in cartItems)
			{
				if (item.UserID == userID && item.ProductID == productID)
				{
					return item;
				}
			}
			return null;
		}

		// Update quantity of an item in the cart
		public void UpdateCart(int userID, int productID, int quantity)
		{
			var cartItems = GetCart(userID);
			var cartItem = FindCartItem(cartItems, userID, productID);

			if (cartItem != null)
			{
				cartItem.Quantity = quantity;
			}

			SaveCart(userID, cartItems); // Save the updated cart items
		}

		// Remove an item from the cart
		public void RemoveFromCart(int userID, int productID)
		{
			var cartItems = GetCart(userID);
			var itemToRemove = FindCartItem(cartItems, userID, productID);
			if (itemToRemove != null)
			{
				cartItems.Remove(itemToRemove);
				SaveCart(userID, cartItems); // Save the updated cart items
			}
		}

		// Clear all items from the cart for a specific user
		public void ClearCart(int userID)
		{
			var session = GetSession();
			session.Remove($"CartItems_{userID}"); // Remove the cart items session data
		}

		// Check out the cart
		public void CheckOut(int userID)
		{
			var transactionModel = new TransactionModel();
			var cart = GetCart(userID);

			// Create a new order and obtain the new transactionGroupID
			int newTransactionGroupID = transactionModel.CreateNewOrder(userID);

			// Place each item in the cart under the same order (transaction group)
			foreach (var item in cart)
			{
				transactionModel.PlaceOrder(userID, item.ProductID, item.Quantity, newTransactionGroupID);
			}

			ClearCart(userID); // Clear the cart after checkout

		}
	}

	public class CartItem
	{
		public int UserID
		{
			get; set;
		}
		public int ProductID
		{
			get; set;
		}
		public string ProductName
		{
			get; set;
		}
		public decimal Price
		{
			get; set;
		}
		public int Quantity
		{
			get; set;
		}
	}
}
