using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly CustomerRepository _customerRepository;
		public CustomerController(CustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}
		#region Get All Customer
		[HttpGet]
		public IActionResult GetAllCustomer()
		{
			var customers = _customerRepository.GetAllCustomer();
			return Ok(customers);
		}
		#endregion
		#region Get Customer By ID
		[HttpGet("{id}")]
		public IActionResult GetCustomerByID(int id)
		{
			var customer = _customerRepository.GetCustomerByPK(id);
			if (customer == null) return NotFound();
			return Ok(customer);
		}
		#endregion
		#region Insert Customer
		[HttpPost]
		public IActionResult InsertCustomer([FromBody] CustomerModel customerModel)
		{
			if(customerModel == null) return BadRequest();
			bool isInserted = _customerRepository.Insert(customerModel);
			if(isInserted) return Ok(new { Message = "Customer Inserted Successfully" });
			return StatusCode(500, "Error occurred");
		}
		#endregion
		#region Update Customer
		[HttpPut("{id}")]
		public IActionResult UpdateCustomer(int id, [FromBody] CustomerModel customerModel)
		{
			if (customerModel == null || id != customerModel.CustomerID) return BadRequest();
			bool isUpdated = _customerRepository.Update(customerModel);
			if (isUpdated) return Ok(new { Message = "Customer Updated Successfully" });
			return StatusCode(500, "Error occurred");
		}
		#endregion
		#region Delete Customer
		[HttpDelete("{id}")]
		public IActionResult DeleteCustomer(int id)
		{
			var isDeleted = _customerRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound();
			}
			return NoContent();
		}
		#endregion
		//#region Customer Count
		//[HttpGet]
		//public IActionResult GetCustomerCount()
		//{
		//	try
		//	{
		//		int result = _customerRepository.GetCountCustomer();
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
