using ConsumeCoffeeShopWebAPI.Helper;
using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	//[CheckAccess]
	public class CityController : Controller
	{
		private readonly string _connectionString;
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;
		public CityController(IConfiguration configuration)
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
			_connectionString = configuration.GetConnectionString("ConnectionString");
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public JsonResult Delete(int cityID)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand cmd = new SqlCommand("PR_LOC_City_Delete", connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			cmd.Parameters.AddWithValue("@CityID", cityID);
			connection.Open();
			int rowAffected = cmd.ExecuteNonQuery();
			return Json(rowAffected);
		}
		[HttpGet]
		public JsonResult Details()
		{
			var cities = new List<City>();
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
					cities.Add(new City
					{
						CityID = Convert.ToInt32(reader["CityID"]),
						CityName = reader["CityName"].ToString(),
						CityCode = reader["CityCode"].ToString()
					});
				}
			}
			return Json(cities);
		}
		#region Get All City
		[HttpGet]
		public IActionResult GetAllCity()
		{
			List<CityModel> list = new List<CityModel>();
			try
			{
				HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/GetAllCities").Result;
				if (response.IsSuccessStatusCode)
				{
					string data = response.Content.ReadAsStringAsync().Result;
					list = JsonConvert.DeserializeObject<List<CityModel>>(data);
				}
				else
				{
					Console.WriteLine($"Error: {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
			}
			return View("GetAllCity", list);
		}
		#endregion
		#region Add Method
		public async Task<IActionResult> Add(string? addID)
		{
			await LoadCountryList();
			int? decryptedCityID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedCityIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedCityID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
			}
			if (decryptedCityID.HasValue)
			{
				var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/GetCityByID/{decryptedCityID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var city = JsonConvert.DeserializeObject<CityModel>(data);
					ViewBag.StateList = await GetStatesByCountryID(city.CountryID);
					return View(city);
				}
			}
			return View(new CityModel());
		}
		#endregion
		#region Save Method
		[HttpPost]
		public async Task<IActionResult> Save(CityModel cityModel)
		{
			if (true)
			{
				var json = JsonConvert.SerializeObject(cityModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (cityModel.CityID == null || cityModel.CityID == 0){
					response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/City/InsertCity", content);
					TempData["SaveMessage"] = "City Insert Successfully";
				}
				else{
					response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/City/UpdateCity/{cityModel.CityID}", content);
					TempData["SaveMessage"] = "City Update Successfully";
				}
				if (response.IsSuccessStatusCode){
					return RedirectToAction("GetAllCity");
				}
			}
			await LoadCountryList();
			return View("Add", cityModel);
		}
		#endregion
		#region Delete City By ID
		[HttpGet]
		public IActionResult DeleteCity(string cityID)
		{
			int decryptedUserID = Convert.ToInt32(UrlEncryptor.Decrypt(cityID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/City/DeleteCityByID/{decryptedUserID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["SaveMessage"] = "City Delete Successfully";
			}
			return RedirectToAction("GetAllCity");
		}
		#endregion
		#region Contry Drop Down
		private async Task LoadCountryList()
		{
			var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/GetCountryDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var countries = JsonConvert.DeserializeObject<List<CountryDropDownModel>>(data);
				ViewBag.CountryList = countries;
			}
		}
		#endregion
		#region Get State By Country(Drop Down)
		[HttpPost]
		public async Task<JsonResult> GetStatesByCountry(int CountryID)
		{
			var states = await GetStatesByCountryID(CountryID);
			return Json(states);
		}

		private async Task<List<StateDropDownModel>> GetStatesByCountryID(int CountryID)
		{
			var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/StateDropDown/{CountryID}");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<List<StateDropDownModel>>(data);
			}
			return new List<StateDropDownModel>();
		}
		#endregion
		#region Get City BY State
		public async Task<IActionResult> CityByState(int? StateID)
		{
			List<CityModel> cities = new List<CityModel>();
			List<CityModel> newCities = new List<CityModel>();

			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/GetAllCities");

				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					newCities = JsonConvert.DeserializeObject<List<CityModel>>(data);
					if (StateID.HasValue)
					{
						foreach (var item in newCities)
						{
							if (item.StateID == StateID)
							{
								cities.Add(item);
							}
						}
					}
					else
					{
						cities = newCities;
					}
				}
				else
				{
					Console.WriteLine($"API Error: {response.StatusCode}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception occurred: {ex.Message}");
			}
			return View("GetAllCity", cities);
		}
		#endregion
	}
}
