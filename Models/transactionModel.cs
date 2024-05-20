using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class TransactionModel
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
		public string ProductName
		{
			get; set;
		}

		public DateTime TransactionDate
		{
			get; set;
		}

		public int PlaceOrder(int? userID, int productID, int quantity)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				string sql = "INSERT INTO transactionTable (UserID, ProductID, transactionQuantity, transactionDate) VALUES (@UserID, @ProductID, @Quantity, @TransactionDate)";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);
				cmd.Parameters.AddWithValue("@ProductID", productID);
				cmd.Parameters.AddWithValue("@Quantity", quantity);
				cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);

				con.Open();
				int rowsAffected = cmd.ExecuteNonQuery();
				con.Close();
				return rowsAffected;
			}
		}


		public static List<TransactionModel> RetrieveUserTransactions(int? userID)
		{
			List<TransactionModel> transactions = new List<TransactionModel>();
			if (userID != null)
			{
				using (SqlConnection con = new SqlConnection(con_string))
				{
					string sql = @"
                    SELECT t.transactionID, t.userID, t.productID, t.transactionQuantity, p.productName, t.transactionDate
                    FROM transactionTable t
                    JOIN productTable p ON t.productID = p.productID
                    WHERE t.userID = @userID";

					SqlCommand cmd = new SqlCommand(sql, con);
					cmd.Parameters.AddWithValue("@userID", userID);

					con.Open();
					SqlDataReader rdr = cmd.ExecuteReader();
					while (rdr.Read())
					{
						TransactionModel transaction = new TransactionModel
						{
							TransactionID = Convert.ToInt32(rdr["transactionID"]),
							UserID = Convert.ToInt32(rdr["userID"]),
							ProductID = Convert.ToInt32(rdr["productID"]),
							Quantity = Convert.ToInt32(rdr["transactionQuantity"]),
							ProductName = rdr["productName"].ToString(),
							TransactionDate = Convert.ToDateTime(rdr["transactionDate"])
						};

						transactions.Add(transaction);
					}
				}
			}
			return transactions;
		}
	}
}