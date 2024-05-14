using System.Data.SqlClient;

namespace CloudApplication.Models
{
	public class userTable
	{
		//ublic static string con_string = "Server = tcp:st10257863-server.database.windows.net,1433;Initial Catalog = CloudDatabase; Persist Security Info=False;User ID = Jamie; Password=window-festive-grandee-dessert!12; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
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

		public int insertUser(userTable a)
		{
			try
			{
				string sql = "INSERT INTO userTable (userName, userSurname, userEmail) VALUES (@Name, @Surname, @Email)";
				SqlCommand cmd = new SqlCommand(sql, con);

				cmd.Parameters.AddWithValue("@Name", a.Name);
				cmd.Parameters.AddWithValue("@Surname", a.Surname);
				cmd.Parameters.AddWithValue("@Email", a.Email);

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

