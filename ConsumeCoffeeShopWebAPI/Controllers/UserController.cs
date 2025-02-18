using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text;
using ConsumeCoffeeShopWebAPI.Helper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
    public class UserController : Controller
    {
		private IConfiguration _configuration;
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;
		public UserController(IConfiguration configuration)
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
			_configuration = configuration;
		}
		#region User List
		[HttpGet]
		public IActionResult UserList()
        {
            List<UserModel> users = new List<UserModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/User/GetAllUsers").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserModel>>(data);
			}
            return View("UserList", users);
        }
        #endregion
        #region Add-Edit
        public async Task<IActionResult> UserAddOrEdit(string? addID)
        {

			int? decryptedCityID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedCityIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedCityID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
			}
			if (decryptedCityID.HasValue)
			{
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/User/GetUserByID/{decryptedCityID}");
                if(response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
					var user = JsonConvert.DeserializeObject<UserModel>(data);
                    return View(user);
				}
            }
            return View(new UserModel());
        }
        #endregion
        #region Save
        [HttpPost]
        public async Task<IActionResult> UserSave(UserModel userModel)
        {

			if (ModelState.IsValid)
            {
				var json = JsonConvert.SerializeObject(userModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (userModel.UserID == null || userModel.UserID == 0)
				{
					response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/User/InsertUser", content);
					TempData["AlertMessage"] = "User Insert Successfully";
				}
				else
				{
					response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/User/UpdateUser/{userModel.UserID}", content);
					TempData["AlertMessage"] = "User Update Successfully";
				}
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("UserList");
				}
            }

            return View("UserAddOrEdit", userModel);
        }
		#endregion
		#region Delete
		[HttpGet]
		public IActionResult UserDelete(string userID)
        {
			int decryptedUserID = Convert.ToInt32(UrlEncryptor.Decrypt(userID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/User/DeleteUser/{decryptedUserID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["AlertMessage"] = "User Delete Successfully";
			}
			return RedirectToAction("UserList");
        }
        #endregion
        #region Excel
        public IActionResult ExportToExcel()
        {
            // Fetch the product data
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            // Create the Excel file in memory
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                // Load the data from the DataTable into the worksheet, starting from cell A1.
                worksheet.Cells["A1"].LoadFromDataTable(table, true);

                // Format the header row
                using (var range = worksheet.Cells["A1:Z1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                }

                // Convert the package to a byte array
                var excelData = package.GetAsByteArray();

                Response.Headers.Add("Content-Disposition", "inline; filename=User List.xlsx");

                // Return the Excel file as a download
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
		#endregion
		#region User Profile
		public IActionResult UserProfile(int UserID)
        {
            UserModel user = new UserModel();
            string connectionString = _configuration.GetConnectionString("ConnectionString");
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			SqlCommand command = connection.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "PR_User_SelectByPK";
			command.Parameters.AddWithValue("@UserID", UserID);
			SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = new UserModel
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Address = reader["Address"].ToString(),
                    Password = reader["Password"].ToString() 
                };
            }
                return View(user); 
        }
		#endregion
	}
}
