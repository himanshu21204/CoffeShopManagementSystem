using System.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class CountryRepository
    {
        private readonly string _connectionString;
        public CountryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        #region Select All Country
        public IEnumerable<CountryModel> SelectAll()
        {
            var countries = new List<CountryModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    countries.Add(new CountryModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString(),
                        CountryCode = reader["CountryCode"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"])
                    });
                }
            }
            return countries;
        }
        #endregion
        #region Select By ID Country
        public CountryModel SelectByPK(int CountryID)
        {
            CountryModel? country = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_SelectByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryID", CountryID);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    country = new CountryModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString(),
                        CountryCode = reader["CountryCode"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"])
                    };
                }
            }
            return country;
        }
        #endregion
        #region Insert Country
        public bool InsertCountry(CountryModel country)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Insert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryName", country.CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", country.CountryCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Update Country
        public bool UpdateCountry(CountryModel country)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_Country_Update", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryID", country.CountryID);
                cmd.Parameters.AddWithValue("@CountryName", country.CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", country.CountryCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Delete Country
        public bool DeleteCountry(int CountryID)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PR_LOC_Country_Delete", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            connection.Open();
            int rowAffected = cmd.ExecuteNonQuery();
            return rowAffected > 0;
        }
        #endregion
    }
}
