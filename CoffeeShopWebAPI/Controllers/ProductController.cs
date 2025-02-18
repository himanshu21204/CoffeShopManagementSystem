using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ProductRepository _productRepository;
		public ProductController(ProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		#region Get All Product
		[HttpGet]
		public IActionResult GetAllProduct()
		{
			var product = _productRepository.SelectAll();
			return Ok(product);
		}
		#endregion
		#region Get Product By ID
		[HttpGet("{id}")]
		public IActionResult GetProductByID(int id)
		{
			var product = _productRepository.SelectBYPK(id);
			if(product == null) return NotFound();
			return Ok(product);
		}
		#endregion
		#region Insert Product
		[HttpPost]
		public IActionResult InsertProduct([FromBody] ProductModel productModel)
		{
			if(productModel == null) return BadRequest();
			bool isInserted = _productRepository.Insert(productModel);
			if (isInserted) return Ok(new { Message = "Product Inserted Successfully" });
			return StatusCode(500, "Error occurred");
		}
		#endregion
		#region Update Product
		[HttpPut("{id}")]
		public IActionResult UpdateProduct(int id,[FromBody] ProductModel productModel)
		{
			if(productModel == null || id != productModel.ProductID) return BadRequest();
			bool isUpdated = _productRepository.Update(productModel);
			if(isUpdated) return Ok(new { Message = "Product Updated Successfully" });
			return NoContent();
		}
		#endregion
		#region Delete Product
		[HttpDelete("{id}")]		
		public IActionResult DeleteProduct(int id)
		{
			var isDeleted = _productRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound();
			}
			return NoContent();
		}
		#endregion
		//#region Product Count
		//[HttpGet]
		//public IActionResult GetProductCount()
		//{
		//	try
		//	{
		//		int result = _productRepository.GetCountProduct();
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
