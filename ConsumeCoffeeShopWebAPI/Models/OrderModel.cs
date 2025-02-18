using System.ComponentModel.DataAnnotations;
using System;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class OrderModel
    {
        //[Key]
        public int OrderID { get; set; }

        //[Required(ErrorMessage = "Order Date is required")]
        public DateTime? OrderDate { get; set; }
        //[Required(ErrorMessage = "Order Number is required")]
        public string OrderNumber { get; set; }

        //[Required(ErrorMessage = "Customer ID is required")]
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string PaymentMode { get; set; } = "Cash";

        //[Required(ErrorMessage = "Total Amount is required")]
        //[Range(1, double.MaxValue, ErrorMessage = "Total Amount must be greater than zero")]
        public double? TotalAmount { get; set; }

        //[Required(ErrorMessage = "Shipping Address is required")]
        public string ShippingAddress { get; set; }

        //[Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
    }
}
