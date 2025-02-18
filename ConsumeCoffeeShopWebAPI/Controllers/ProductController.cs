using ConsumeCoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text;
using ConsumeCoffeeShopWebAPI.Helper;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ConsumeCoffeeShopWebAPI.Controllers
{
	[CheckAccess]
	public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        Uri baseAddress = new Uri("http://localhost:38334/api");
		private readonly HttpClient _httpClient;
		public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        #region Product List
        [HttpGet]
        public IActionResult ProductList()
        {
            List<ProductModel> products = new List<ProductModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Product/GetAllProduct").Result;
            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                products = JsonConvert.DeserializeObject<List<ProductModel>>(data);
            }
            return View(products);
        }
        #endregion
        #region Add-Edit
        public async Task<IActionResult> ProductAddOrEdit(string? addID)
        {
			await UserDropDown();
			int? decryptedProductID = null;
			// Decrypt only if UserID is not null or empty
			if (!string.IsNullOrEmpty(addID))
			{
				string decryptedProductIDString = UrlEncryptor.Decrypt(addID); // Decrypt the encrypted UserID
				decryptedProductID = int.Parse(decryptedProductIDString); // Convert decrypted string to integer
			}
			if (decryptedProductID.HasValue)
			{
                HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Product/GetProductByID/{decryptedProductID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    ProductModel product = JsonConvert.DeserializeObject<ProductModel>(data);
                    return View(product);
                }
            }
            return View(new ProductModel());
        }
        #endregion
        #region Save
        [HttpPost]
        public async Task<IActionResult> ProductSave(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(productModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (productModel.ProductID == null || productModel.ProductID == 0)
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Product/InsertProduct", content);
                    TempData["AlertMessage"] = "Product Insert Successfully";
                }
                else
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Product/UpdateProduct/{productModel.ProductID}", content);
                    TempData["AlertMessage"] = "Product Update Successfully";
                }
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProductList");
                }
            }
            await UserDropDown();
            return View("ProductAddOrEdit", productModel);
		}
        #endregion
        #region Delete
        [HttpGet]
        public IActionResult ProductDelete(string productID)
        {
			int decryptedProductID = Convert.ToInt32(UrlEncryptor.Decrypt(productID));
			HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Product/DeleteProduct/{decryptedProductID}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Product Delete Successfully";
            }
            return RedirectToAction("ProductList");
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
                ViewBag.UserDropDown = users;
            }
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
            command.CommandText = "PR_Product_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            // Create the Excel file in memory
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

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

                Response.Headers.Add("Content-Disposition", "inline; filename=Product List.xlsx");

                // Return the Excel file as a download
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
        #endregion
    }
}
 