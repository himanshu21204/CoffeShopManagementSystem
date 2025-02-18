using System.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class StateRepository
    {
        private readonly string _connectionString;

        public StateRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        #region Select All State
        public IEnumerable<StateModel> StateGetAll()
        {
            var states = new List<StateModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new StateModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        StateName = reader["StateName"].ToString(),
                        StateCode = reader["StateCode"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"]),
                        CityCount = Convert.ToInt32(reader["CityCount"])
                    });
                }
            }
            return states;
        }
        #endregion
        #region Select By ID State
        public StateModel SelectByID(int StateID)
        {
            StateModel? stateModel = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_SelectByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("StateID", StateID);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stateModel = new StateModel
                    {
                        StateID = Convert.ToInt32(reader["StateID"]),
                        CountryID = Convert.ToInt32(reader["CountryID"]),
                        StateName = reader["StateName"].ToString(),
                        StateCode = reader["StateCode"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["ModifiedDate"]),
						CityCount = Convert.ToInt32(reader["CityCount"])
					};
                }
            }
            return stateModel;
        }
        #endregion
        #region Insert State
        public bool InsertState(StateModel state)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Insert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CountryID", state.CountryID);
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Update State
        public bool UpdateState(StateModel state)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Update", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", state.StateID);
                cmd.Parameters.AddWithValue("@CountryID", state.CountryID);
                cmd.Parameters.AddWithValue("@StateName", state.StateName);
                cmd.Parameters.AddWithValue("@StateCode", state.StateCode);
                connection.Open();
                int affectedRow = cmd.ExecuteNonQuery();
                return affectedRow > 0;
            }
        }
        #endregion
        #region Delete State
        public bool DeleteState(int StateID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_LOC_State_Delete", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@StateID", StateID);
                connection.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                return rowAffected > 0;
            }
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
    }
}
