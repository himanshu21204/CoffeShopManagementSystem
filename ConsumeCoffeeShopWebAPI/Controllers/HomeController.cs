using ConsumeCoffeeShopWebAPI.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	public class HomeController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;

		public HomeController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}
		public async Task<IActionResult> Index()
		{
			//GetCustomerCount();
			//GetProductCount();
			//GetOrderCount();

			DashboardModel dashboard = new DashboardModel();
			HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Dashboard/GetAllDashboard");
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				dashboard = JsonConvert.DeserializeObject<DashboardModel>(data);
			}
			return View(dashboard);
		}

		public IActionResult Login()
		{
			return View();
		}
		#region Customer Count
		public void GetCustomerCount()
		{
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Customer/GetCustomerCount").Result;
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				var customerCount = JsonConvert.DeserializeObject<int>(data);
				ViewBag.CustomerCount = customerCount;
			}
			else
			{
				ViewBag.Error = "Unable to retrieve customer count.";
			}
		}
		#endregion
		#region Customer Count
		public void GetProductCount()
		{
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Product/GetProductCount").Result;
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				var productCount = JsonConvert.DeserializeObject<int>(data);
				ViewBag.ProductCount = productCount;
			}
			else
			{
				ViewBag.Error = "Unable to retrieve customer count.";
			}
		}
		#endregion
		#region Order Count
		public void GetOrderCount()
		{
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Order/GetOrderCount").Result;
			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				var orderCount = JsonConvert.DeserializeObject<int>(data);
				ViewBag.OrderCount = orderCount;
			}
			else
			{
				ViewBag.Error = "Unable to retrieve order count.";
			}
		}
		#endregion
	}
}
