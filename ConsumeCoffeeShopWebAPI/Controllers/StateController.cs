using ConsumeCoffeeShopWebAPI.Helper;
using ConsumeCoffeeShopWebAPI.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class StateController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;

		public StateController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}
		#region Get All State
		[HttpGet]
		public IActionResult GetAllState()
		{
			List<StateModel> states = new List<StateModel>();
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/State/GetAllStates").Result;
			if(response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				List<StateModel> temp = JsonConvert.DeserializeObject<List<StateModel>>(data);
				states = temp;
			}
			return View("GetAllState",states);
		}
		#endregion
		#region Add Method
		public async Task<IActionResult> Add(string? addID)
		{
			await LoadCountryList();
			int? decryptedStateID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedStateIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedStateID = int.Parse(decryptedStateIDString); // Convert decrypted string to integer
			}
			if (decryptedStateID.HasValue)
			{
				var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/State/GetStateByID/{decryptedStateID}");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var state = JsonConvert.DeserializeObject<StateModel>(data);
					return View(state);
				}
			}
			return View(new StateModel());
		}
		#endregion
		#region Save Method
		[HttpPost]
		public async Task<IActionResult> Save(StateModel stateModel)
		{
			if (ModelState.IsValid)
			{
				var json = JsonConvert.SerializeObject(stateModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (stateModel.StateID == null || stateModel.StateID == 0)
				{
					response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/State/InsertState", content);
					TempData["SaveMessage"] = "State Insert Successfully";
				}
				else
				{
					response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/State/UpdateState/{stateModel.StateID}", content);
					TempData["SaveMessage"] = "State Update Successfully";
				}
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("GetAllState");
				}
			}
			await LoadCountryList();
			return View("Add", stateModel);
		}
		#endregion
		#region Delete State By ID
		[HttpGet]
		public IActionResult DeleteState(string stateID)
		{
			int decryptedStateID = Convert.ToInt32(UrlEncryptor.Decrypt(stateID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/State/DeleteStateByID/{decryptedStateID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["SaveMessage"] = "State Delete Successfully";
			}
			return RedirectToAction("GetAllState");
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
	}
}
