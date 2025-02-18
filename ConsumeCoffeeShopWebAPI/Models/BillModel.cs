using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class BillModel
    {
        public int? BillID { get; set; }

        //[Required(ErrorMessage = "Bill Number is Required")]
        //[StringLength(50, ErrorMessage = "Bill Number cannot exceed 50 characters")]
        public string BillNumber { get; set; }

        //[Required(ErrorMessage = "Bill Date is Required")]
        //[DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime? BillDate { get; set; }

        //[Required(ErrorMessage = "Select Order ID")]
        //[Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive number")]
        public int OrderID { get; set; }

        //[Required(ErrorMessage = "Total Amount is Required")]
        public decimal? TotalAmount { get; set; }

        //[Range(0, 60, ErrorMessage = "Discount cannot be negative")]
        public decimal? Discount { get; set; }

        //[Required]
        //[Range(1, double.MaxValue, ErrorMessage = "Net Amount cannot be negative")]
        public decimal? NetAmount { get; set; }

        //[Required(ErrorMessage = "Select USer ID")]
        public int UserID { get; set; }
        public string? UserName { get; set; }

    }
    public class BillDropDownModel
    {
        public int BillID { get; set; }
        public string BillName { get; set; }
    }
}
