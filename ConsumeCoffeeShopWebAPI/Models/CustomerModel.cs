using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class CustomerModel
    {
        //[Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

        //[Required(ErrorMessage = "Customer Name is required.")]
        public string CustomerName { get; set; }

        //[Required(ErrorMessage = "Home Address is required.")]
        public string HomeAddress { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Mobile Number is required.")]
        //[Phone(ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNo { get; set; }

        //[Required]
        //[StringLength(15, ErrorMessage = "GST Number can't be longer than 15 characters.")]
        public string GST_NO { get; set; }

        //[Required(ErrorMessage = "City Name is required.")]
        public string CityName { get; set; }

        //[Required(ErrorMessage = "Pin Code is required.")]
        //[StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be exactly 6 characters.")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Pin Code must be numeric.")]
        public string PinCode { get; set; }

        //[Required]
        //[Range(1, double.MaxValue, ErrorMessage = "Net Amount must be greater than zero.")]
        public decimal NetAmount { get; set; }

        //[Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }
        public string? UserName { get; set; }
		public int OrderCount { get; set; }
	}
    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
