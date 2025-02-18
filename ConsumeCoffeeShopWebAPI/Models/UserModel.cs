using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class UserModel
    {
        //[Key] 
        public int UserID { get; set; }

        //[Required(ErrorMessage = "Username is required.")]
        //[StringLength(100, ErrorMessage = "Username can't be longer than 100 characters.")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required.")]
        //[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        //[PasswordPropertyText]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Mobile number is required.")]
        //[Phone(ErrorMessage = "Invalid phone number.")]
        public string MobileNo { get; set; }

        //[Required]
        //[StringLength(200, ErrorMessage = "Address can't be longer than 200 characters.")]
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
