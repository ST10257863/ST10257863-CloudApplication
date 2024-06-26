﻿using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class userModel
	{
		public static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog=ST10257863-database;Persist Security Info=False;User ID=Jamie;Password=window-festive-grandee-dessert!12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		public static SqlConnection con = new SqlConnection(con_string);

		public string Name
		{
			get; set;
		}

		public string Surname
		{
			get; set;
		}

		public string Email
		{
			get; set;
		}

		public string Password
		{
			get; set;
		}

		public int insertUser(userModel a)
		{
			try
			{
				string sql = "INSERT INTO userTable (userName, userSurname, userEmail, userPassword) VALUES (@Name, @Surname, @Email, @Password)";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", a.Name);
				cmd.Parameters.AddWithValue("@Surname", a.Surname);
				cmd.Parameters.AddWithValue("@Email", a.Email);
				cmd.Parameters.AddWithValue("@Password", a.Password);

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

		public int SelectUser(string password, string name)
		{
			int userID = -1; // Default value if user is not found
			using (SqlConnection con = new SqlConnection(con_string))
			{
				string sql = "SELECT userID FROM userTable WHERE userPassword = @Password AND userName = @Name";
				SqlCommand cmd = new SqlCommand(sql, con);
				cmd.Parameters.AddWithValue("@Password", password);
				cmd.Parameters.AddWithValue("@Name", name);
				try
				{
					con.Open();
					object result = cmd.ExecuteScalar();
					if (result != null && result != DBNull.Value)
					{
						userID = Convert.ToInt32(result);
					}
				}
				catch (Exception ex)
				{
					// Log the exception or handle it appropriately
					// For now, rethrow the exception
					throw ex;
				}
			}
			return userID;
		}
	}
}