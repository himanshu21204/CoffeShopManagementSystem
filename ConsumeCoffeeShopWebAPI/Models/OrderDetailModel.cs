using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class OrderDetailModel
    {
        //[Required]
        public int? OrderDetailID { get; set; }

        //[Required]
        public int OrderID { get; set; }
        public string? OrderNumber { get; set; }

        //[Required]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }

        //[Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int? Quantity { get; set; }

        //[Required]
        //[Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double? Amount { get; set; }

        //[Required]
        //[Range(1, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        public double? TotalAmount { get; set; }

        //[Required]
        public int UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class OrderDetailDropDownModel
    {
        public int OrderDetailID { get; set; }
        public string OrderDetailName { get; set; }
    }
	public class ErrorResponse
	{
		public string Type { get; set; }
		public string Title { get; set; }
		public int Status { get; set; }
		public Dictionary<string, List<string>> Errors { get; set; }
		public string TraceId { get; set; }
	}
}
