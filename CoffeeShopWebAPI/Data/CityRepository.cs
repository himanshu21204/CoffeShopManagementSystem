using System.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class CityRepository
    {
        private readonly string _connectionString;
        public CityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        #region Select All City
        public IEnumerable<CityModel> SelectAll()
        {
            var cities = new List<CityModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cities.Add(new CityModel
                    {
                        CityID = Convert.ToInt32(reader["CityID"]),
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CityName = reader["CityName"].ToString(),
                        CityCode = reader["CityCode"].ToString(),
                        StateName = reader["StateName"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"])
                    });
                }
            }
            return cities;
        }
        #endregion
        #region Select By ID City
        public CityModel SelectByPK(int CityID)
        {
            CityModel? city = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_SelectByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CityID", CityID);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    city = new CityModel
                    {
                        CityID = Convert.ToInt32(reader["CityID"]),
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CityName = reader["CityName"].ToString(),
                        CityCode = reader["CityCode"].ToString(),
                        StateName = reader["StateName"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"])
                    };
                }
            }
            return city;
        }
        #endregion
        #region Insert City
        public bool Insert(CityModel city)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Insert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", city.StateID);
                cmd.Parameters.AddWithValue("@CountryID", city.CountryID);
                cmd.Parameters.AddWithValue("@CityName", city.CityName);
                cmd.Parameters.AddWithValue("@CityCode", city.CityCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Update City
        public bool Update(CityModel city)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_City_Update", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CityID", city.CityID);
                cmd.Parameters.AddWithValue("@StateID", city.StateID);
                cmd.Parameters.AddWithValue("@CountryID", city.CountryID);
                cmd.Parameters.AddWithValue("@CityName", city.CityName);
                cmd.Parameters.AddWithValue("@CityCode", city.CityCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Delete City
        public bool Delete(int CityID)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("PR_LOC_City_Delete", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@CityID", CityID);
            connection.Open();
            int rowAffected = cmd.ExecuteNonQuery();
            return rowAffected > 0;
        }
        #endregion
        #region Country Drop Down
        public IEnumerable<CountryDropDownModel> CountryDropDown()
        {
            var countrys = new List<CountryDropDownModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("PR_LOC_Country_SelectComboBox", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    countrys.Add(new CountryDropDownModel
                    {
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        CountryName = reader["CountryName"].ToString()
                    });
                }
            }
            return countrys;
        }
        #endregion
        #region State Drop Down By using Country ID
        public IEnumerable<StateDropDownModel> StateDropDown(int? CountryID)
        {
            var states = new List<StateDropDownModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("PR_LOC_State_SelectComboBoxByCountryID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("CountryID", CountryID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new StateDropDownModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        StateName = reader["StateName"].ToString()
                    });
                }
            }
            return states;
        }
        #endregion
    }
}