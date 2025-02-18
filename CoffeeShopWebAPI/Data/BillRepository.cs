using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace CoffeeShopWebAPI.Data
{
	public class BillRepository
	{
		private readonly string _connectionString;
		public BillRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		#region Select All Bill
		public IEnumerable<BillModel> SelectAll()
		{
			var bills = new List<BillModel>();
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Bills_SelectAll", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					bills.Add(new BillModel
					{
						BillID = Convert.ToInt32(reader["BillID"]),
						BillNumber = reader["BillNumber"].ToString(),
						BillDate = Convert.ToDateTime(reader["BillDate"]),
						OrderID = Convert.ToInt32(reader["OrderID"]),
						TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
						Discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : null,
						NetAmount = Convert.ToDecimal(reader["NetAmount"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString()
					});
				}
			}
			return bills;
		}
		#endregion
		#region Select Bill By ID
		public BillModel SelectByPK(int billID)
		{
			BillModel? bill = null;
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Bills_SelectByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("BillID", billID);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				if (reader.Read())
				{
					bill = new BillModel
					{
						BillID = Convert.ToInt32(reader["BillID"]),
						BillNumber = reader["BillNumber"].ToString(),
						BillDate = Convert.ToDateTime(reader["BillDate"]),
						OrderID = Convert.ToInt32(reader["OrderID"]),
						TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
						Discount = reader["Discount"] != DBNull.Value ? Convert.ToDecimal(reader["Discount"]) : null,
						NetAmount = Convert.ToDecimal(reader["NetAmount"]),
						UserID = Convert.ToInt32(reader["UserID"]),
						UserName = reader["UserName"].ToString()
					};
				}
			}
			return bill;
		}
		#endregion
		#region Insert Bill
		public bool Insert(BillModel billModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Bills_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
				command.Parameters.AddWithValue("BillDate", billModel.BillDate);
				command.Parameters.AddWithValue("OrderID", billModel.OrderID);
				command.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
				command.Parameters.AddWithValue("Discount", billModel.Discount);
				command.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
				command.Parameters.AddWithValue("UserID", billModel.UserID);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Update Bill
		public bool Update(BillModel billModel)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Bills_UpdateByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("BillID", billModel.BillID);
				command.Parameters.AddWithValue("BillNumber", billModel.BillNumber);
				command.Parameters.AddWithValue("BillDate", billModel.BillDate);
				command.Parameters.AddWithValue("OrderID", billModel.OrderID);
				command.Parameters.AddWithValue("TotalAmount", billModel.TotalAmount);
				command.Parameters.AddWithValue("Discount", billModel.Discount);
				command.Parameters.AddWithValue("NetAmount", billModel.NetAmount);
				command.Parameters.AddWithValue("UserID", billModel.UserID);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
		#region Delete Bill
		public bool Delete(int BillID)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand("PR_LOC_Bills_DeleteByPK", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				command.Parameters.AddWithValue("BillID", BillID);
				connection.Open();
				int affectedRows = command.ExecuteNonQuery();
				return affectedRows > 0;
			}
		}
		#endregion
	}
}
