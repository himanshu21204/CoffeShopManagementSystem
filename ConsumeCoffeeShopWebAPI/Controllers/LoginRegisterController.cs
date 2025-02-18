using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using static ConsumeCoffeeShopWebAPI.Models.LoginRegister;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	public class LoginRegisterController : Controller
	{
		private IConfiguration _configuration;

		public LoginRegisterController(IConfiguration _configuration)
		{
			this._configuration = _configuration;
		}
		public IActionResult Register()
		{
			return View();
		}
		#region Register
		public IActionResult UserRegister(UserRegisterModel userRegisterModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					string connectionString = this._configuration.GetConnectionString("ConnectionString");
					SqlConnection sqlConnection = new SqlConnection(connectionString);
					sqlConnection.Open();
					SqlCommand sqlCommand = sqlConnection.CreateCommand();
					sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
					sqlCommand.CommandText = "PR_User_Register";
					sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
					sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
					sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
					sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
					sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
					sqlCommand.ExecuteNonQuery();
					return RedirectToAction("Login", "LoginRegister");
				}
			}
			catch (Exception e)
			{
				TempData["ErrorMessage"] = e.Message;
				return RedirectToAction("Register");
			}
			return RedirectToAction("Register");
		}
		#endregion
		public IActionResult Login()
		{
			if(HttpContext.Session.GetString("UserID") != null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
		[HttpPost]
		public IActionResult Login(UserLoginModel userLoginModel)
		{
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();

                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";

                    // Add input parameters
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName ?? string.Empty;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password ?? string.Empty;

                    // Add output parameter to capture login success/failure
                    SqlParameter loginSuccessParam = new SqlParameter("@LoginSuccess", SqlDbType.Bit);
                    loginSuccessParam.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(loginSuccessParam);

                    // Execute the command and read the data
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    // Load data if the reader has rows
                    DataTable dataTable = new DataTable();
                    if (sqlDataReader.HasRows)
                    {
                        dataTable.Load(sqlDataReader);
                    }

                    // Close the reader first before checking the output parameter
                    sqlDataReader.Close();

                    // Retrieve the value of @LoginSuccess after the reader is closed
                    bool loginSuccess = (bool)sqlCommand.Parameters["@LoginSuccess"].Value;

                    if (loginSuccess && dataTable.Rows.Count > 0)
                    {
                        // If login successful, load user data from DataTable
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid username or password.";
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "LoginRegister");
		}
	}
}
