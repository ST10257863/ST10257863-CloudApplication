using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class CartModel
	{
		// SQL Connection string
		private static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog=ST10257863-database;Persist Security Info=False;User ID=Jamie;Password=window-festive-grandee-dessert!12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		// Method to get the cart items for a specific user
		public List<CartItem> GetCart(int userID)
		{
			var cartItems = new List<CartItem>();

			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = "SELECT c.UserID, c.ProductID, p.ProductName, p.productPrice, c.Quantity FROM cartTable c JOIN productTable p ON c.productID = p.productID WHERE UserID = @UserID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);

				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						CartItem item = new CartItem
						{
							UserID = Convert.ToInt32(rdr["UserID"]),
							ProductID = Convert.ToInt32(rdr["ProductID"]),
							ProductName = rdr["ProductName"].ToString(),
							Price = Convert.ToDecimal(rdr["productPrice"]),
							Quantity = Convert.ToInt32(rdr["Quantity"])
						};
						cartItems.Add(item);
					}
				}

				con.Close();
			}

			return cartItems;
		}

		// Method to add a product to the cart for a specific user
		public int AddToCart(int userID, int productID, int quantity)
		{
			if (ProductModel.checkProductAvailablility(productID) == true)
			{
				using (SqlConnection con = new SqlConnection(con_string))
				{
					string sql = @"
                INSERT INTO cartTable (UserID, ProductID, Quantity) 
                VALUES (@UserID, @ProductID, @Quantity)";
					SqlCommand cmd = new SqlCommand(sql, con);
					cmd.Parameters.AddWithValue("@UserID", userID);
					cmd.Parameters.AddWithValue("@ProductID", productID);
					cmd.Parameters.AddWithValue("@Quantity", quantity);

					con.Open();
					int rowsAffected = cmd.ExecuteNonQuery();
					con.Close();
					return rowsAffected;
				}
			}
			else
			{
				return 0;
			}
		}

		// Method to update the quantity of an item in the cart
		public void UpdateCart(int userID, int productID, int quantity)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = @"
                UPDATE cartTable 
                SET Quantity = @Quantity 
                WHERE UserID = @UserID AND ProductID = @ProductID";

				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);
				cmd.Parameters.AddWithValue("@ProductID", productID);
				cmd.Parameters.AddWithValue("@Quantity", quantity);

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		// Method to remove an item from the cart
		public void RemoveFromCart(int userID, int productID)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = @"
                DELETE FROM cartTable 
                WHERE UserID = @UserID AND ProductID = @ProductID";

				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);
				cmd.Parameters.AddWithValue("@ProductID", productID);

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		// Method to clear all items from the cart for a specific user
		public void ClearCart(int userID)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = @"
                DELETE FROM cartTable 
                WHERE UserID = @UserID";

				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);

				cmd.ExecuteNonQuery();

				con.Close();
			}
		}

		// Check out the cart
		public void CheckOut(int userID)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{

				var transactionModel = new TransactionModel();
				var cart = GetCart(userID);
				string sql = @"
                DELETE FROM cartTable 
                WHERE UserID = @UserID";

				// Create a new order and obtain the new transactionGroupID
				int newTransactionGroupID = transactionModel.CreateNewOrder(userID);
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);

				// Place each item in the cart under the same order (transaction group)
				foreach (var item in cart)
				{
					transactionModel.PlaceOrder(userID, item.ProductID, item.Quantity, newTransactionGroupID);
				}

				ClearCart(userID); // Clear the cart after checkout
				con.Close();
			}
		}

		public Boolean CheckItemInCart(int productID)
		{
			Boolean isItemInCart = false;
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = "SELECT * FROM cartTable WHERE ProductID = @ProductID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@ProductID", productID);

				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					isItemInCart = true;
				}

				con.Close();
			}
			return isItemInCart;
		}
		public int GetQuantity(int userID, int productID)
		{
			int quantity = 0;
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = "SELECT Quantity FROM cartTable WHERE UserID = @UserID AND ProductID = @ProductID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);
				cmd.Parameters.AddWithValue("@ProductID", productID);

				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					quantity = Convert.ToInt32(rdr["Quantity"]);
				}

				con.Close();
			}
			return quantity;
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
}