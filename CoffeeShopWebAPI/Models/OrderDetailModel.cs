using System.ComponentModel.DataAnnotations;

namespace CoffeeShopWebAPI.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }

        public int OrderID { get; set; }
		public string? OrderNumber { get; set; }

		public int ProductID { get; set; }
        public string? ProductName { get; set; }

        public int? Quantity { get; set; }

        public double? Amount { get; set; }

        public double? TotalAmount { get; set; }

        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class OrderDetailDropDownModel
    {
        public int OrderDetailID { get; set; }
        public string OrderDetailName { get; set; }
    }
}
