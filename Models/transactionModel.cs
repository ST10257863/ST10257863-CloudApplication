using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class transactionModel
	{
		public static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog=ST10257863-database;Persist Security Info=False;User ID=Jamie;Password=window-festive-grandee-dessert!12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		public static SqlConnection con = new SqlConnection(con_string);

		public int TransactionID
		{
			get; set;
		}
		public int UserID
		{
			get; set;
		}
		public int ProductID
		{
			get; set;
		}
		public int Quantity
		{
			get; set;
		}

		public int PlaceOrder(int? userID, int productID, int quantity)
		{
			try
			{
				using (con)
				{
					string sql = "INSERT INTO transactionTable (userID, productID, transactionQuantity) VALUES (@UserID, @ProductID, @Quantity)";
					using (SqlCommand cmd = new SqlCommand(sql, con))
					{
						cmd.Parameters.AddWithValue("@UserID", userID);
						cmd.Parameters.AddWithValue("@ProductID", productID);
						cmd.Parameters.AddWithValue("@Quantity", quantity);

						con.Open();
						int rowsAffected = cmd.ExecuteNonQuery();
						con.Close();
						return rowsAffected;
					}
				}
			}
			catch (Exception ex)
			{
				// Log the exception (if logging is set up) or handle it as needed
				throw; // Rethrow the exception to be handled by the calling code
			}
		}


		public static List<transactionModel> RetrieveUserTransactions(int? userID)
		{
			List<transactionModel> transactions = new List<transactionModel>();
			if (userID != null)
			{

				using (SqlConnection con = new SqlConnection(con_string))
				{
					string sql = "SELECT * FROM transactionTable WHERE userID = @userID";
					SqlCommand cmd = new SqlCommand(sql, con);
					cmd.Parameters.AddWithValue("@userID", userID);

					con.Open();
					SqlDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())

					{
						transactionModel transaction = new transactionModel();
						transaction.TransactionID = Convert.ToInt32(rdr["transactionID"]);
						transaction.UserID = Convert.ToInt32(rdr["userID"]);
						transaction.ProductID = Convert.ToInt32(rdr["productID"]);
						transaction.Quantity = Convert.ToInt32(rdr["transactionQuantity"]);

						transactions.Add(transaction);
					}
				}
			}
			return transactions;
		}
	}
}