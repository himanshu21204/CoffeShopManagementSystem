using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShopWebAPI.Data
{
	public class OrderRepository
	{
		private readonly string _connectionString;
		public OrderRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#region Select All Order
		public IEnumerable<OrderModel> SelectAll()
		{
			List<OrderModel> orders = new List<OrderModel>();
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_SelectAll", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while(reader.Read())
				{
					orders.Add(new OrderModel()
					{
						OrderID = Convert.ToInt32(reader["OrderID"]),
						OrderNumber = reader["OrderNumber"].ToString(),
						OrderDate = Convert.ToDateTime(reader["OrderDate"]),
						CustomerID = Convert.ToInt32(reader["CustomerID"]),
						CustomerName = reader["CustomerName"].ToString(),
						PaymentMode = reader["PaymentMode"].ToString(),
						TotalAmount = Convert.ToDouble(reader["TotalAmount"]),
						ShippingAddress = reader["ShippingAddress"].ToString(),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString()
					});
				}
			}
			return orders;
		}
		#endregion
		#region Select Order By ID
		public OrderModel SelectByPK(int OrderID)
		{
			OrderModel? order = null;
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_SelectByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("OrderID", OrderID);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					order = new OrderModel()
					{
						OrderID = Convert.ToInt32(reader["OrderID"]),
						OrderNumber = reader["OrderNumber"].ToString(),
						OrderDate = Convert.ToDateTime(reader["OrderDate"]),
						CustomerID = Convert.ToInt32(reader["CustomerID"]),
						CustomerName = reader["CustomerName"].ToString(),
						PaymentMode = reader["PaymentMode"].ToString(),
						TotalAmount = Convert.ToDouble(reader["TotalAmount"]),
						ShippingAddress = reader["ShippingAddress"].ToString(),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString()
					};
				}
			}
			return order;
		}
		#endregion
		#region Insert Order
		public bool Insert(OrderModel orderModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
				command.Parameters.AddWithValue("OrderNumber", orderModel.OrderNumber);
				command.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
				command.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
				command.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
				command.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
				command.Parameters.AddWithValue("UserID", orderModel.UserID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Update Order
		public bool Update(OrderModel orderModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_UpdateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("OrderID", orderModel.OrderID);
				command.Parameters.AddWithValue("OrderDate", orderModel.OrderDate);
				command.Parameters.AddWithValue("OrderNumber", orderModel.OrderNumber);
				command.Parameters.AddWithValue("CustomerID", orderModel.CustomerID);
				command.Parameters.AddWithValue("TotalAmount", orderModel.TotalAmount);
				command.Parameters.AddWithValue("PaymentMode", orderModel.PaymentMode);
				command.Parameters.AddWithValue("ShippingAddress", orderModel.ShippingAddress);
				command.Parameters.AddWithValue("UserID", orderModel.UserID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		#region Delete Order
		public bool Delete(int OrderID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Order_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("OrderID", OrderID);
				connection.Open();
				int affectedRow = command.ExecuteNonQuery();
				return affectedRow > 0;
			}
		}
		#endregion
		//#region Get Order Count
		//public int GetCountOrder()
		//{
		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(_connectionString))
		//		{
		//			SqlCommand command = new SqlCommand("PR_LOC_Order_CountOrders", connection)
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
