using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class ProductModel
	{
		public static string con_string = "Server=tcp:st10257863-server.database.windows.net,1433;Initial Catalog=ST10257863-database;Persist Security Info=False;User ID=Jamie;Password=window-festive-grandee-dessert!12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
		public static SqlConnection con = new SqlConnection(con_string);

		public int ProductID
		{
			get; set;
		}
		public string Name
		{
			get; set;
		}
		public decimal Price
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

		public int InsertProduct(ProductModel product)
		{
			try
			{
				string sql = "INSERT INTO productTable (productName, productPrice, productCategory, productAvailability) VALUES (@Name, @Price, @Category, @Availability)";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", product.Name);
				cmd.Parameters.AddWithValue("@Price", product.Price);
				string category = (product.Category.Length <= 50) ? product.Category : product.Category.Substring(0, 50);
				cmd.Parameters.AddWithValue("@Category", category);
				cmd.Parameters.AddWithValue("@Availability", product.Availability);

				con.Open();
				int rowsAffected = cmd.ExecuteNonQuery();
				con.Close();
				return rowsAffected;
			}
			catch (Exception ex)
			{
				con.Close();
				throw ex;
			}
		}

		public static List<ProductModel> RetrieveProducts(string sortOrder)
		{
			List<ProductModel> products = new List<ProductModel>();

			using (SqlConnection con = new SqlConnection(con_string))
			{
				string orderClause;
				switch (sortOrder)
				{
					case "price_asc":
						orderClause = "ORDER BY productPrice ASC";
						break;
					case "price_desc":
						orderClause = "ORDER BY productPrice DESC";
						break;
					default:
						orderClause = "ORDER BY productID";
						break;
				}

				string sql = $@"
                    SELECT ProductID, productName, productPrice, productCategory, productAvailability
                    FROM productTable
                    {orderClause}";

				SqlCommand cmd = new SqlCommand(sql, con);

				con.Open();
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					ProductModel product = new ProductModel
					{
						ProductID = Convert.ToInt32(rdr["ProductID"]),
						Name = rdr["productName"].ToString(),
						Price = Convert.ToDecimal(rdr["productPrice"]),
						Category = rdr["productCategory"].ToString(),
						Availability = rdr["productAvailability"].ToString()
					};

					products.Add(product);
				}
			}

			return products;
		}

		public static ProductModel RetrieveProductByID(int productID)
		{
			ProductModel product = null;

			using (SqlConnection con = new SqlConnection(con_string))
			{
				string sql = "SELECT ProductID, productName, productPrice, productCategory, productAvailability FROM productTable WHERE ProductID = @ProductID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@ProductID", productID);

				con.Open();
				SqlDataReader rdr = cmd.ExecuteReader();
				if (rdr.Read())
				{
					product = new ProductModel
					{
						ProductID = Convert.ToInt32(rdr["ProductID"]),
						Name = rdr["productName"].ToString(),
						Price = Convert.ToDecimal(rdr["productPrice"]),
						Category = rdr["productCategory"].ToString(),
						Availability = rdr["productAvailability"].ToString()
					};
				}
				con.Close();
			}

			return product;
		}
	}
}
