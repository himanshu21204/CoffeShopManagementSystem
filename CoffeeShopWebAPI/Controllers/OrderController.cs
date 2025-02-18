using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly OrderRepository _OrderRepository;
		public OrderController(OrderRepository OrderRepository)
		{
			_OrderRepository = OrderRepository;
		}
		#region Get All Order
		[HttpGet]
		public IActionResult GetAllOrders()
		{
			var Orders = _OrderRepository.SelectAll();
			return Ok(Orders);
		}
		#endregion
		#region Get Order By ID
		[HttpGet("{id}")]
		public IActionResult GetOrderByID(int id)
		{
			OrderModel Order = _OrderRepository.SelectByPK(id);
			if (Order == null)
			{
				return NotFound();
			}
			return Ok(Order);
		}
		#endregion
		#region Insert Order
		[HttpPost]
		public IActionResult InsertOrder([FromBody] OrderModel OrderModel)
		{
			if (OrderModel == null) return BadRequest();
			bool isInserted = _OrderRepository.Insert(OrderModel);
			if (isInserted) return Ok(new { Message = "Order Inserted Successfully" });
			return StatusCode(500, "Error occurred");
		}
		#endregion
		#region Update Order
		[HttpPut("{id}")]
		public IActionResult UpdateOrder(int id, [FromBody] OrderModel OrderModel)
		{
			if (OrderModel == null || id != OrderModel.OrderID) return BadRequest();
			bool isUpdated = _OrderRepository.Update(OrderModel);
			if (isUpdated) return Ok(new { Message = "Order Updated Successfully" });
			return NoContent();
		}
		#endregion
		#region Delete Order
		[HttpDelete("{id}")]
		public IActionResult DeleteOrder(int id)
		{
			var isDeleted = _OrderRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound();
			}
			return NoContent();
		}
		#endregion
		//#region Order Count
		//[HttpGet]
		//public IActionResult GetOrderCount()
		//{
		//	try
		//	{
		//		int result = _OrderRepository.GetCountOrder();
		//		return Ok(result);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, new { Message = "An error occurred while retrieving data.", Error = ex.Message });
		//	}
		//}
		//#endregion
	}
}
