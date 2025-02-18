using CoffeeShopWebAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class DropDownController : ControllerBase
	{
		private readonly DropDownRepository _dropDownRepository;
		public DropDownController(DropDownRepository dropDownRepository)
		{
			_dropDownRepository = dropDownRepository;
		}
		#region User Drop Down
		[HttpGet]
		public IActionResult GetUserDropDown()
		{
			var users = _dropDownRepository.UserDropDown();
			return Ok(users);
		}
		#endregion
		#region Order Drop Down
		[HttpGet]
		public IActionResult GetOrderDropDown()
		{
			var orders = _dropDownRepository.OrderDropDown();
			return Ok(orders);
		}
		#endregion
		#region Customer Drop Down
		[HttpGet]
		public IActionResult GetCustomerDropDown()
		{
			var customers = _dropDownRepository.CustomerDropDown();
			return Ok(customers);
		}
		#endregion
		#region Product Drop Down
		[HttpGet]
		public IActionResult GetProductDropDown()
		{
			var products = _dropDownRepository.ProductDropDown();
			return Ok(products);
		}
		#endregion
	}
}
