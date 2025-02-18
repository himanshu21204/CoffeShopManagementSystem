using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShopWebAPI.Data
{
	public class DropDownRepository
	{
		public readonly string _connectionString;
		public DropDownRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#region User Drop Down
		public IEnumerable<UserDropDownModel> UserDropDown()
		{
			var users = new List<UserDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_User_DropDown", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					users.Add(new UserDropDownModel
					{
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString()
					});
				}
			}
			return users;
		}
		#endregion
		#region Order Drop Down
		public IEnumerable<OrderDropDownModel> OrderDropDown()
		{
			var orders = new List<OrderDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_DropDown", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					orders.Add(new OrderDropDownModel
					{
						OrderID = Convert.ToInt32(reader["OrderID"]),
						OrderNumber = reader["OrderNumber"].ToString()
					});
				}
			}
			return orders;
		}
		#endregion
		#region Product Drop Down
		public IEnumerable<ProductDropDownModel> ProductDropDown()
		{
			var products = new List<ProductDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Product_DropDown", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					products.Add(new ProductDropDownModel
					{
						ProductID = Convert.ToInt32(reader["ProductID"]),
						ProductName = reader["ProductName"].ToString()
					});
				}
			}
			return products;
		}
		#endregion
		#region Customer Drop Down
		public IEnumerable<CustomerDropDownModel> CustomerDropDown()
		{
			var customers = new List<CustomerDropDownModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_DropDown", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					customers.Add(new CustomerDropDownModel
					{
						CustomerID = Convert.ToInt32(reader["CustomerID"]),
						CustomerName = reader["CustomerName"].ToString()
					});
				}
			}
			return customers;
		}
		#endregion
	}
}
