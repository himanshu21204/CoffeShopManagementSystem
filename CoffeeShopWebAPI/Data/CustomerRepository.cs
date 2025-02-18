using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace CoffeeShopWebAPI.Data
{
	public class CustomerRepository
	{
		private readonly string _connectionString;
		public CustomerRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#region Select All Customer
		public IEnumerable<CustomerModel> GetAllCustomer()
		{
			List<CustomerModel> customers = new List<CustomerModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_SelectAll", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					customers.Add(new CustomerModel
					{
						CustomerID = Convert.ToInt32(reader["CustomerID"]),
						CustomerName = reader["CustomerName"].ToString(),
						HomeAddress = reader["HomeAddress"].ToString(),
						Email = reader["Email"].ToString(),
						MobileNo = reader["MobileNO"].ToString(),
						GST_NO = reader["GST_NO"].ToString(),
						CityName = reader["CityName"].ToString(),
						PinCode = reader["PinCode"].ToString(),
						NetAmount = Convert.ToInt32(reader["NetAmount"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						OrderCount = Convert.ToInt32(reader["OrderCount"])
					});
				}
			}
			return customers;
		}
		#endregion
		#region Select Customer By ID
		public CustomerModel GetCustomerByPK(int CustomerID)
		{
			CustomerModel? customer = null;
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_SelectByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("CustomerID", CustomerID);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					customer = new CustomerModel
					{
						CustomerID = Convert.ToInt32(reader["CustomerID"]),
						CustomerName = reader["CustomerName"].ToString(),
						HomeAddress = reader["HomeAddress"].ToString(),
						Email = reader["Email"].ToString(),
						MobileNo = reader["MobileNO"].ToString(),
						GST_NO = reader["GST_NO"].ToString(),
						CityName = reader["CityName"].ToString(),
						PinCode = reader["PinCode"].ToString(),
						NetAmount = Convert.ToInt32(reader["NetAmount"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString(),
						//OrderCount = Convert.ToInt32(reader["OrderCount"])
					};
				}
			}
			return customer;
		}
		#endregion
		#region Insert Customer
		public bool Insert(CustomerModel customer)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("CustomerName", customer.CustomerName);
				command.Parameters.AddWithValue("HomeAddress", customer.HomeAddress);
				command.Parameters.AddWithValue("Email", customer.Email);
				command.Parameters.AddWithValue("MobileNo", customer.MobileNo);
				command.Parameters.AddWithValue("GST_NO", customer.GST_NO);
				command.Parameters.AddWithValue("CityName", customer.CityName);
				command.Parameters.AddWithValue("PinCode", customer.PinCode);
				command.Parameters.AddWithValue("NetAmount", customer.NetAmount);
				command.Parameters.AddWithValue("UserID", customer.UserID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Update Customer
		public bool Update(CustomerModel customer)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_UpdateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
				command.Parameters.AddWithValue("CustomerName", customer.CustomerName);
				command.Parameters.AddWithValue("HomeAddress", customer.HomeAddress);
				command.Parameters.AddWithValue("Email", customer.Email);
				command.Parameters.AddWithValue("MobileNo", customer.MobileNo);
				command.Parameters.AddWithValue("GST_NO", customer.GST_NO);
				command.Parameters.AddWithValue("CityName", customer.CityName);
				command.Parameters.AddWithValue("PinCode", customer.PinCode);
				command.Parameters.AddWithValue("NetAmount", customer.NetAmount);
				command.Parameters.AddWithValue("UserID", customer.UserID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Delete Customer
		public bool Delete(int CustomerID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Customer_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("CustomerID", CustomerID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		//#region Get Customer Count
		//public int GetCountCustomer()
		//{
		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(_connectionString))
		//		{
		//			SqlCommand command = new SqlCommand("PR_LOC_Customer_CountCustomer", connection)
		//			{
		//				CommandType = CommandType.StoredProcedure
		//			};

		//			connection.Open();

		//			var result = command.ExecuteScalar();
		//			return (int)(result == null ? 0 : result);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return 0;
		//	}
		//}
		//#endregion
	}
}
