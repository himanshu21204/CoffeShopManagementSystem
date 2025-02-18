using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        //[Required(ErrorMessage = "Name is Required")]
        public string ProductName { get; set; }
        //[Required(ErrorMessage = "Price is Required")]
        public double? ProductPrice { get; set; }
        //[Required(ErrorMessage = "Code is Required")]
        public string ProductCode { get; set; }
        //[Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "User ID is Required")]
        public int UserID { get; set; }
    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
