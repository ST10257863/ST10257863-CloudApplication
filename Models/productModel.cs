using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class ProductModel
	{
		public static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog=ST10257863-database;Persist Security Info=False;User ID=Jamie;Password=window-festive-grandee-dessert!12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
		public int ImageID
		{
			get; set;
		}
		public string ImageName
		{
			get; set;
		}
		public string ImageUrl
		{
			get; set;
		}

		public int InsertProduct(ProductModel product)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(con_string))
				{
					// SQL query to insert product details into productTable
					string sqlProduct = "INSERT INTO productTable (productName, productPrice, productCategory, productAvailability) VALUES (@Name, @Price, @Category, @Availability); SELECT SCOPE_IDENTITY();";
					SqlCommand cmdProduct = new SqlCommand(sqlProduct, con);

					// Adding parameters to the cmdProduct command
					cmdProduct.Parameters.AddWithValue("@Name", product.Name);
					cmdProduct.Parameters.AddWithValue("@Price", product.Price);
					cmdProduct.Parameters.AddWithValue("@Category", product.Category);
					cmdProduct.Parameters.AddWithValue("@Availability", product.Availability);

					con.Open();
					// Execute the command and retrieve the new ProductID
					product.ProductID = Convert.ToInt32(cmdProduct.ExecuteScalar());

					// SQL query to insert image details into productImageTable
					string sqlImage = "INSERT INTO productImageTable (productID, ImageName, ImageUrl) VALUES (@ProductID, @ImageName, @ImageUrl)";
					SqlCommand cmdImage = new SqlCommand(sqlImage, con);

					// Adding parameters to the cmdImage command
					cmdImage.Parameters.AddWithValue("@ProductID", product.ProductID);
					cmdImage.Parameters.AddWithValue("@ImageName", product.ImageName);
					cmdImage.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);

					// Execute the command and get the number of rows affected
					int rowsAffected = cmdImage.ExecuteNonQuery();
					con.Close();

					return rowsAffected;
				}
			}
			catch (Exception ex)
			{
				con.Close();
				throw ex;
			}
		}

		public static List<ProductModel> RetrieveProductsImages()
		{
			List<ProductModel> products = new List<ProductModel>();

			using (SqlConnection con = new SqlConnection(con_string))
			{
				string sql = @"
            SELECT p.ProductID, p.productName, i.ImageID, i.ImageName, i.ImageUrl
            FROM productTable p
            LEFT JOIN productImageTable i ON p.ProductID = i.ProductID";
				SqlCommand cmd = new SqlCommand(sql, con);

				con.Open();
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					ProductModel product = new ProductModel
					{
						ProductID = Convert.ToInt32(rdr["ProductID"]),
						Name = rdr["productName"].ToString(),
						ImageName = rdr["ImageName"]?.ToString(),
						ImageUrl = rdr["ImageUrl"]?.ToString()
					};

					products.Add(product);
				}
				con.Close();
			}

			return products;
		}
		public static List<ProductModel> RetrieveProducts(String sortOrder)
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
				string sql = @"
                    SELECT p.ProductID, p.productName, p.productPrice, p.productCategory, p.productAvailability, i.ImageID, i.ImageName, i.ImageUrl
                    FROM productTable p
                    LEFT JOIN productImageTable i ON p.ProductID = i.ProductID";
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
						Availability = rdr["productAvailability"].ToString(),
						ImageID = rdr["ImageID"] != DBNull.Value ? Convert.ToInt32(rdr["ImageID"]) : 0,
						ImageName = rdr["ImageName"]?.ToString(),
						ImageUrl = rdr["ImageUrl"]?.ToString()
					};

					products.Add(product);
				}
				con.Close();
			}

			return products;
		}

		public static Boolean checkProductAvailablility(int productID)
		{
			Boolean isAvailable = false;
			using (SqlConnection con = new SqlConnection(con_string))
			{
				string sql = "SELECT productAvailability FROM productTable WHERE ProductID = @ProductID";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@ProductID", productID);

				con.Open();
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					if (rdr["productAvailability"].ToString() == "Available")
					{
						isAvailable = true;
					}
				}
				con.Close();
			}
			return isAvailable;
		}
	}
}
