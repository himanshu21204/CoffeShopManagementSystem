using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using OfficeOpenXml;
using Newtonsoft.Json;
using System.Text;
using ConsumeCoffeeShopWebAPI.Helper;
using System.Net.Http;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class BillController : Controller
	{
		Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;
		private IConfiguration _configuration;

		public BillController(IConfiguration _configuration)
		{
			this._configuration = _configuration;
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
		}

		#region Bill List
		public IActionResult BillList()
		{
			try
			{
				List<BillModel> bills = new List<BillModel>();
				HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Bill/GetAllBills").Result;
				if (response.IsSuccessStatusCode)
				{
					var data = response.Content.ReadAsStringAsync().Result;
					bills = JsonConvert.DeserializeObject<List<BillModel>>(data);
				}
				return View(bills);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in BillList: {ex.Message}");
				return View(new List<BillModel>());
			}
		}
		#endregion

		#region Add-Edit
		public async Task<IActionResult> BillAddOrEdit(string? addID)
		{
			try
			{
				await UserDropDown();
				await OrderDropDown();

				int? decryptedCityID = null;
				if (!string.IsNullOrEmpty(addID))
				{
					string decryptedCityIDString = UrlEncryptor.Decrypt(addID);
					decryptedCityID = int.Parse(decryptedCityIDString);
				}
				ViewBag.OrderList = ViewBag.OrderList ?? new List<OrderDropDownModel>();
				ViewBag.UserList = ViewBag.UserList ?? new List<UserDropDownModel>();

				if (decryptedCityID.HasValue)
				{
					var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Bill/GetBillByID/{decryptedCityID}");
					if (response.IsSuccessStatusCode)
					{
						var data = await response.Content.ReadAsStringAsync();
						var bill = JsonConvert.DeserializeObject<BillModel>(data);
						return View(bill);
					}
				}
				return View(new BillModel());
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in BillAddOrEdit: {ex.Message}");
				return View(new BillModel());
			}
		}
		#endregion

		#region Save
		[HttpPost]
		public async Task<IActionResult> BillSave(BillModel billModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var json = JsonConvert.SerializeObject(billModel);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					HttpResponseMessage response;

					if (billModel.UserID == null || billModel.UserID == 0)
					{
						response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Bill/InsertBill", content);
						TempData["AlertMessage"] = "Bill Inserted Successfully";
					}
					else
					{
						response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Bill/UpdateBill/{billModel.BillID}", content);
						TempData["AlertMessage"] = "Bill Updated Successfully";
					}

					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("BillList");
					}
				}

				await UserDropDown();
				await OrderDropDown();
				return View("BillAddOrEdit", billModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in BillSave: {ex.Message}");
				await UserDropDown();
				await OrderDropDown();
				return View("BillAddOrEdit", billModel);
			}
		}
		#endregion

		#region Delete
		[HttpGet]
		public IActionResult BillDelete(string billID)
		{
			try
			{
				int decryptedBillID = Convert.ToInt32(UrlEncryptor.Decrypt(billID));
				HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Bill/DeleteBill/{decryptedBillID}").Result;
				if (response.IsSuccessStatusCode)
				{
					TempData["AlertMessage"] = "Bill Deleted Successfully";
				}
				return RedirectToAction("BillList");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in BillDelete: {ex.Message}");
				return RedirectToAction("BillList");
			}
		}
		#endregion

		#region Order Drop Down
		public async Task OrderDropDown()
		{
			try
			{
				var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetOrderDropDown");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var orderList = JsonConvert.DeserializeObject<List<OrderDropDownModel>>(data);
					ViewBag.OrderList = orderList;
				}
				else
				{
					ViewBag.OrderList = new List<OrderDropDownModel>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in OrderDropDown: {ex.Message}");
				ViewBag.OrderList = new List<OrderDropDownModel>();
			}
		}
		#endregion

		#region User Drop Down
		public async Task UserDropDown()
		{
			try
			{
				var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetUserDropDown");
				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var userList = JsonConvert.DeserializeObject<List<UserDropDownModel>>(data);
					ViewBag.UserList = userList;
				}
				else
				{
					ViewBag.UserList = new List<UserDropDownModel>();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in UserDropDown: {ex.Message}");
				ViewBag.UserList = new List<UserDropDownModel>();
			}
		}
		#endregion

		#region Excel
		public IActionResult ExportToExcel()
		{
			try
			{
				List<BillModel> bills = new List<BillModel>();
				HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Bill/GetAllBills").Result;

				if (response.IsSuccessStatusCode)
				{
					var data = response.Content.ReadAsStringAsync().Result;
					bills = JsonConvert.DeserializeObject<List<BillModel>>(data);
				}

				DataTable billTable = new DataTable();
				billTable.Columns.Add("BillID", typeof(int));
				billTable.Columns.Add("BillNumber", typeof(string));
				billTable.Columns.Add("BillDate", typeof(DateTime));
				billTable.Columns.Add("OrderID", typeof(int));
				billTable.Columns.Add("TotalAmount", typeof(decimal));
				billTable.Columns.Add("Discount", typeof(decimal));
				billTable.Columns.Add("NetAmount", typeof(decimal));
				billTable.Columns.Add("UserID", typeof(int));

				foreach (var bill in bills)
				{
					billTable.Rows.Add(
						bill.BillID,
						bill.BillNumber,
						bill.BillDate ?? DateTime.MinValue,
						bill.OrderID,
						bill.TotalAmount ?? 0.0m,
						bill.Discount ?? 0.0m,
						bill.NetAmount ?? 0.0m,
						bill.UserID
					);
				}

				using (var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Bills");
					worksheet.Cells["A1"].LoadFromDataTable(billTable, true);

					using (var range = worksheet.Cells["A1:Z1"])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
					}

					var excelData = package.GetAsByteArray();
					Response.Headers.Add("Content-Disposition", "inline; filename=Bill List.xlsx");
					return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in ExportToExcel: {ex.Message}");
				return RedirectToAction("BillList");
			}
		}
		#endregion
	}
}
