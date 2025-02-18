using System.ComponentModel.DataAnnotations;
using System;

namespace CoffeeShopWebAPI.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentMode { get; set; } = "Cash";
        public double? TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; } 
        public int UserID { get; set; }
        public string? UserName { get; set; }
		public int CustomerID { get; set; }
		public string? CustomerName { get; set; }
	}
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
    }
}
