using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using Newtonsoft.Json;
using System.Text;
using ConsumeCoffeeShopWebAPI.Helper;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class OrderDetailController : Controller
    {
        private IConfiguration _configuration;
        Uri baseAddress = new Uri("http://localhost:38334/api");
        HttpClient _httpClient;
        public OrderDetailController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        #region Order Details List
        [HttpGet]
        public IActionResult OrderDetailList()
        {
            List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/OrderDetail/GetAllOrderDetails").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                orderDetails = JsonConvert.DeserializeObject<List<OrderDetailModel>>(data);
            }
            return View(orderDetails);
        }
        #endregion
        #region Add-Edit
        public async Task<IActionResult> OrderDetailAddOrEdit(string? addID)
        {
            await OrderDropDown();
            await UserDropDown();
            await ProductDropDown();
			int? decryptedOrderDetailID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedOrderDetailIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedOrderDetailID = int.Parse(decryptedOrderDetailIDString); // Convert decrypted string to integer
			}

			if (decryptedOrderDetailID <= 0 || decryptedOrderDetailID == null)
			{
				ViewBag.AddOrEdit = "Add";
			}
			else
			{
				ViewBag.AddOrEdit = "Edit";
			}
			if (decryptedOrderDetailID.HasValue)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/OrderDetail/GetOrderDetailByID/{decryptedOrderDetailID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    OrderDetailModel orderDetail = JsonConvert.DeserializeObject<OrderDetailModel>(data);
                    return View(orderDetail);
                }
            }
			return View();
        }
        #endregion
        #region Save
        [HttpPost]
		public async Task<IActionResult> OrderDetailSave(OrderDetailModel orderDetailModel)
		{
			if (ModelState.IsValid)
			{
				var json = JsonConvert.SerializeObject(orderDetailModel);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response;

				if (orderDetailModel.OrderDetailID == null || orderDetailModel.OrderDetailID == 0)
				{
					response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/OrderDetail/InsertOrderDetail", content);
					var errorContent = await response.Content.ReadAsStringAsync();

					if (!response.IsSuccessStatusCode)
					{
						// Deserialize error content (assuming it's JSON)
						var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);

						if (errorResponse?.Errors != null)
						{
							// Pass errors to the view using ViewData
							ViewData["ApiErrors"] = errorResponse.Errors;
						}
						else
						{
							ViewData["ApiErrors"] = new Dictionary<string, List<string>>()
					{
						{ "General", new List<string> { "An unknown error occurred." } }
					};
						}
					}
					else
					{
						TempData["AlertMessage"] = "Order Detail Insert Successfully";
						return RedirectToAction("OrderDetailList");
					}
				}
				else
				{
					response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/OrderDetail/UpdateOrderDetail/{orderDetailModel.OrderDetailID}", content);
					TempData["AlertMessage"] = "Order Detail Update Successfully";

					if (response.IsSuccessStatusCode)
					{
						return RedirectToAction("OrderDetailList");
					}
				}
			}

			await ProductDropDown();
			await UserDropDown();
			await OrderDropDown();

			return View("OrderDetailAddOrEdit", orderDetailModel);
		}

		#endregion
		#region Delete
		[HttpGet]
        public IActionResult OrderDetailDelete(string orderDetailID)
        {
			int decryptedOrderDetailID = Convert.ToInt32(UrlEncryptor.Decrypt(orderDetailID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/OrderDetail/DeleteOrderDetail/{decryptedOrderDetailID}").Result;
			if (response.IsSuccessStatusCode)
			{
				TempData["AlertMessage"] = "Order Detail Delete Successfully";
			}
			return RedirectToAction("OrderDetailList");
        }
		#endregion
		#region User Drop Down
		public async Task UserDropDown()
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetUserDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(data);
				ViewBag.UserList = users;
			}
		}
		#endregion
		#region Product Drop Down
		public async Task ProductDropDown()
        {
			HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetProductDropDown");
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				var productList = JsonConvert.DeserializeObject<List<ProductDropDownModel>>(data);
			    ViewBag.ProductList = productList;
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
				Console.WriteLine(ex.Message);
				ViewBag.OrderList = new List<OrderDropDownModel>();
			}
		}
		#endregion
		#region Excel
		public IActionResult ExportToExcel()
        {
			List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();
			HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/OrderDetail/GetAllOrderDetails").Result;

			if (response.IsSuccessStatusCode)
			{
				var data = response.Content.ReadAsStringAsync().Result;
				orderDetails = JsonConvert.DeserializeObject<List<OrderDetailModel>>(data);
			}
			DataTable table = new DataTable();
			table.Columns.Add("OrderDetailID", typeof(int));
			table.Columns.Add("OrderID", typeof(int));
			table.Columns.Add("ProductID", typeof(int));
			table.Columns.Add("Quantity", typeof(int));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("UserID", typeof(int));
			foreach (var order in orderDetails)
			{
				table.Rows.Add(
					order.OrderDetailID,
					order.OrderID,
					order.ProductID,
					order.Quantity ?? 0,
					order.Amount ?? 0.0,
					order.TotalAmount ?? 0.0,
					order.UserID
				);
			}

			// Create the Excel file in memory
			using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("OrderDetails");

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

                Response.Headers.Add("Content-Disposition", "inline; filename=Order Detail List.xlsx");

                // Return the Excel file as a download
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
        #endregion
    }
}
