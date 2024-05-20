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

		public int TransactionGroupID
		{
			get; set;
		}

		public int PlaceOrder(int? userID, int productID, int quantity, int transactionGroupID)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();

				string sql = "INSERT INTO transactionTable (UserID, ProductID, transactionQuantity, transactionDate, transactionGroupID) VALUES (@UserID, @ProductID, @Quantity, @TransactionDate, @TransactionGroupID)";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@UserID", userID);
				cmd.Parameters.AddWithValue("@ProductID", productID);
				cmd.Parameters.AddWithValue("@Quantity", quantity);
				cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
				cmd.Parameters.AddWithValue("@TransactionGroupID", transactionGroupID);

				int transactionID = Convert.ToInt32(cmd.ExecuteScalar());

				con.Close();

				return transactionID;
			}
		}

		public int CreateNewOrder(int userID)
		{
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();
				string getLastTransactionGroupIDQuery = "SELECT TOP 1 transactionGroupID FROM transactionTable ORDER BY transactionGroupID DESC";
				SqlCommand getLastTransactionGroupIDCmd = new SqlCommand(getLastTransactionGroupIDQuery, con);
				int lastTransactionGroupID = 0;
				object result = getLastTransactionGroupIDCmd.ExecuteScalar();
				if (result != null && result != DBNull.Value)
				{
					lastTransactionGroupID = Convert.ToInt32(result);
				}
				int newTransactionGroupID = lastTransactionGroupID + 1;
				return newTransactionGroupID;
			}
		}


		public List<TransactionModel> RetrieveUserTransactions(int? userID)
		{
			List<TransactionModel> transactions = new List<TransactionModel>();
			using (SqlConnection con = new SqlConnection(con_string))
			{
				con.Open();
				string sql = "SELECT t.transactionID, t.userID, t.productID, t.transactionQuantity, p.productName, t.transactionDate, t.transactionGroupID FROM transactionTable t JOIN productTable p ON t.productID = p.productID WHERE t.userID = @userID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@userID", userID);
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
						TransactionDate = Convert.ToDateTime(rdr["transactionDate"]),
						TransactionGroupID = Convert.ToInt32(rdr["transactionGroupID"])

					};
					transactions.Add(transaction);
				}
			}
			return transactions;
		}
	}
}