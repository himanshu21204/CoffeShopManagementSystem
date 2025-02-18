using System.ComponentModel.DataAnnotations;

namespace ConsumeCoffeeShopWebAPI.Models
{
	public class Mail
	{
		public string Email { get; set; }

		public string Name { get; set; }

		public string Subject { get; set; }

		public string Message { get; set; }
	}
}
