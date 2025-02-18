using System.ComponentModel.DataAnnotations;

namespace CoffeeShopWebAPI.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public double? ProductPrice { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int StockQuantity { get; set; }
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
