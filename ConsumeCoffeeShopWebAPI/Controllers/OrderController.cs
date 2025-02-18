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
	public class OrderController : Controller
    {
        private IConfiguration _configuration;
        Uri baseAddress = new Uri("http://localhost:38334/api");
        HttpClient _httpClient;
        public OrderController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        #region Order List
        [HttpGet]
        public IActionResult OrderList()
        {
            List<OrderModel> orders = new List<OrderModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Order/GetAllOrders").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                orders = JsonConvert.DeserializeObject<List<OrderModel>>(data);
            }
            return View(orders);
        }
        #endregion
        #region Add-Edit
        public async Task<IActionResult> OrderAddOrEdit(string? addID)
        {
            await CustomerDropDown();
            await UserDropDown();
			int? decryptedOrderID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedOrderIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedOrderID = int.Parse(decryptedOrderIDString); // Convert decrypted string to integer
			}
			if (decryptedOrderID.HasValue)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Order/GetOrderByID/{decryptedOrderID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<OrderModel>(data);
                    return View(order);
                }
            }
            return View(new OrderModel());
        }
        #endregion
        #region Save
        [HttpPost]
        public async Task<IActionResult> OrderSave(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(orderModel);
                var content = new StringContent(json,Encoding.UTF8,"application/json");
                HttpResponseMessage response;
                if(orderModel.OrderID == null || orderModel.OrderID == 0)
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Order/InsertOrder",content);
					TempData["AlertMessage"] = "Order Insert Successfully";
				}
				else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Order/UpdateOrder/{orderModel.OrderID}", content);
					TempData["AlertMessage"] = "Order Update Successfully";
				}
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("OrderList");
                }
            }
			await CustomerDropDown();
			await UserDropDown();
			return View("OrderAddOrEdit", orderModel);
        }
        #endregion
        #region Delete
        [HttpGet]
        public IActionResult OrderDelete(string orderID)
        {
			int decryptedOrderID = Convert.ToInt32(UrlEncryptor.Decrypt(orderID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Order/DeleteOrder/{decryptedOrderID}").Result;
            if (response.IsSuccessStatusCode)
            {
				TempData["AlertMessage"] = "Order Delete Successfully";
			}
            return RedirectToAction("OrderList");
        }
        #endregion
        #region Customer Drop Down
        public async Task CustomerDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetCustomerDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                List<CustomerDropDownModel> customerList = JsonConvert.DeserializeObject<List<CustomerDropDownModel>>(data);
				ViewBag.CustomerList = customerList;
            }
        }
        #endregion
        #region User Drop Down
        public async Task UserDropDown()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/DropDown/GetUserDropDown");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                List<UserDropDownModel> userList = JsonConvert.DeserializeObject<List<UserDropDownModel>>(data);
                ViewBag.UserList = userList;
            }
        }
		#endregion
		#region Get Order Bu Cuatomer
		public async Task<IActionResult> OrderByCustomer(int? CustomerID)
		{
			List<OrderModel> orders = new List<OrderModel>();
			List<OrderModel> newOrders = new List<OrderModel>();

			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Order/GetAllOrders");

				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					newOrders = JsonConvert.DeserializeObject<List<OrderModel>>(data);
					if (CustomerID.HasValue)
					{
						foreach (var item in newOrders)
						{
							if (item.CustomerID == CustomerID)
							{
								orders.Add(item);
							}
						}
					}
					else
					{
						orders = newOrders;
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
			return View("OrderList", orders);
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
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            // Create the Excel file in memory
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

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

                Response.Headers.Add("Content-Disposition", "inline; filename=Order List.xlsx");

                // Return the Excel file as a download
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
        #endregion
    }
}
