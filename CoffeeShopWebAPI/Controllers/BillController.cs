using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class BillController : ControllerBase
	{
		private readonly BillRepository _billRepository;

		public BillController(BillRepository billRepository)
		{
			_billRepository = billRepository;
		}

		#region Get All Bills
		[HttpGet]
		public IActionResult GetAllBills()
		{
			IEnumerable<BillModel> bills = _billRepository.SelectAll();
			return Ok(bills);
		}
		#endregion

		#region Get Bill By ID
		[HttpGet("{id}")]
		public IActionResult GetBillByID(int id)
		{
			BillModel bill = _billRepository.SelectByPK(id);
			if (bill == null)
			{
				return NotFound(new { Message = "Bill not found" });
			}
			return Ok(bill);
		}
		#endregion

		#region Insert Bill
		[HttpPost]
		public IActionResult InsertBill([FromBody] BillModel billModel)
		{
			if (billModel == null || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			bool isInserted = _billRepository.Insert(billModel);
			if (isInserted)
			{
				return Ok(new { Message = "Bill inserted successfully" });
			}
			return StatusCode(500, new { Message = "Error occurred while inserting bill" });
		}
		#endregion

		#region Update Bill
		[HttpPut("{id}")]
		public IActionResult UpdateBill(int id, [FromBody] BillModel billModel)
		{
			if (billModel == null || id != billModel.BillID || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			bool isUpdated = _billRepository.Update(billModel);
			if (isUpdated)
			{
				return Ok(new { Message = "Bill updated successfully" });
			}
			return StatusCode(500, new { Message = "Error occurred while updating bill" });
		}
		#endregion

		#region Delete Bill
		[HttpDelete("{id}")]
		public IActionResult DeleteBill(int id)
		{
			bool isDeleted = _billRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound(new { Message = "Bill not found or could not be deleted" });
			}
			return NoContent();
		}
		#endregion
	}
}
