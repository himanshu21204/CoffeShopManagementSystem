using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace CoffeeShopWebAPI.Data
{
	public class UserRepository
	{
		private readonly string _connectionString;
		public UserRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#region Select All User
		public IEnumerable<UserModel> SelectAll()
		{
			var users = new List<UserModel>();
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_SelectAll", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					users.Add(new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						Email = reader["Email"].ToString(),
						Password = reader["Password"].ToString(),
						MobileNo = reader["MobileNo"].ToString(),
						Address = reader["Address"].ToString(),
						IsActive = Convert.ToBoolean(reader["IsActive"])
					});
				}
			}
			return users;
		}
		#endregion
		#region Select User By ID
		public UserModel SelectByPK(int UserID)
		{
			UserModel? user = null;
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_SelectByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("UserID", UserID);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					user = new UserModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						Email = reader["Email"].ToString(),
						Password = reader["Password"].ToString(),
						MobileNo = reader["MobileNo"].ToString(),
						Address = reader["Address"].ToString(),
						IsActive = Convert.ToBoolean(reader["IsActive"])
					};
				}
			}
			return user;
		}
		#endregion
		#region Insert User
		public bool Insert(UserModel userModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("UserName", userModel.UserName);
				command.Parameters.AddWithValue("Email", userModel.Email);
				command.Parameters.AddWithValue("Password", userModel.Password);
				command.Parameters.AddWithValue("MobileNo", userModel.MobileNo);
				command.Parameters.AddWithValue("Address", userModel.@Address);
				command.Parameters.AddWithValue("IsActive", true);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Update User
		public bool Update(UserModel userModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_UpdateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("UserID", userModel.UserID);
				command.Parameters.AddWithValue("UserName", userModel.UserName);
				command.Parameters.AddWithValue("Email", userModel.Email);
				command.Parameters.AddWithValue("Password", userModel.Password);
				command.Parameters.AddWithValue("MobileNo", userModel.MobileNo);
				command.Parameters.AddWithValue("Address", userModel.@Address);
				command.Parameters.AddWithValue("IsActive", true);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Delete User
		public bool Delete(int UserID)
		{
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("UserID", UserID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
	}
}
