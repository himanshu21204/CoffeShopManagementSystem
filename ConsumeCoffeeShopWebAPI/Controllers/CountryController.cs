using ConsumeCoffeeShopWebAPI.Helper;
using ConsumeCoffeeShopWebAPI.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class CountryController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;

		public CountryController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}
		#region Get All Country
		public IActionResult GetAllCountry()
		{
			List<CountryModel> countrys = new List<CountryModel>();
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Country/GetAllCountries").Result;
			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				List<CountryModel>? temp = JsonConvert.DeserializeObject<List<CountryModel>>(data);
				countrys = temp;
			}
			return View("GetAllCountry",countrys);
		}
		#endregion
		#region Add Method
		public async Task<IActionResult> Add(string? addID)
		{
			int? decryptedCountryID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedCountryIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedCountryID = int.Parse(decryptedCountryIDString); // Convert decrypted string to integer
			}
			if (decryptedCountryID.HasValue)
			{
				var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Country/GetCountryByID/{decryptedCountryID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var country = JsonConvert.DeserializeObject<CountryModel>(data);
					return View(country);
				}
			}
			return View(new CountryModel());
		}
		#endregion
		#region Save Method
		[HttpPost]
		public async Task<IActionResult> Save(CountryModel CountryModel)
		{
			if (ModelState.IsValid)
			{
				var json = JsonConvert.SerializeObject(CountryModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (CountryModel.CountryID == null || CountryModel.CountryID == 0)
				{
					response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Country/InsertCountry", content);
					TempData["SaveMessage"] = "Country Insert Successfully";
				}
				else
				{
					response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Country/UpdateCountry/{CountryModel.CountryID}", content);
					TempData["SaveMessage"] = "Country Update Successfully";
				}
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("GetAllCountry");
				}
			}
			return View("Add", CountryModel);
		}
		#endregion
		#region Delete Country By ID
		[HttpGet]
		public IActionResult DeleteCountry(string countryID)
		{
			int decryptedCountryID = Convert.ToInt32(UrlEncryptor.Decrypt(countryID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Country/DeleteCountryByID/{decryptedCountryID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["SaveMessage"] = "Country Delete Successfully";
			}
			return RedirectToAction("GetAllCountry");
		}
		#endregion
	}
}
