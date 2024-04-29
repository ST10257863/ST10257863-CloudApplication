using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class productTable
	{
		public static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog = CloudDatabase; Persist Security Info=False;User ID = Jamie; Password=window-festive-grandee-dessert!12; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
		public static SqlConnection con = new SqlConnection(con_string);

		public int ProductID
		{
			get; set;
		}

		public string Name
		{
			get; set;
		}

		public string Price
		{
			get; set;
		}

		public string Category
		{
			get; set;
		}

		public string Availability
		{
			get; set;
		}

		public int insertProduct(productTable product)
		{
			try
			{
				string sql = "INSERT INTO productTable (productName, productPrice, productCategory, productAvailability) VALUES (@Name, @Price, @Category, @Availability)";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", product.Name);
				cmd.Parameters.AddWithValue("@Price", product.Price);
				cmd.Parameters.AddWithValue("@Category", product.Category);
				cmd.Parameters.AddWithValue("@Availability", product.Availability);

				con.Open();
				int rowsAffected = cmd.ExecuteNonQuery();
				con.Close();
				return rowsAffected;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
