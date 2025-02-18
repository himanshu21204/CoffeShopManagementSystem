using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserRepository _userRepository;
		public UserController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		#region Get All User
		[HttpGet]
		public IActionResult GetAllUsers()
		{
			var users = _userRepository.SelectAll();
			return Ok(users);
		}
		#endregion
		#region Get User By ID
		[HttpGet("{id}")]
		public IActionResult GetUserByID(int id)
		{
			UserModel user = _userRepository.SelectByPK(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}
		#endregion
		#region Insert User
		[HttpPost]
		public IActionResult InsertUser([FromBody] UserModel userModel)
		{
			if (userModel == null) return BadRequest();
			bool isInserted = _userRepository.Insert(userModel);
			if (isInserted) return Ok(new { Message = "User Inserted Successfully" });
			return StatusCode(500, "Error occurred");
		}
		#endregion
		#region Update User
		[HttpPut("{id}")]
		public IActionResult UpdateUser(int id,[FromBody] UserModel userModel)
		{
			if(userModel == null || id != userModel.UserID) return BadRequest();
			bool isUpdated = _userRepository.Update(userModel);
			if (isUpdated) return Ok(new { Message = "User Updated Successfully" });
			return NoContent();
		}
		#endregion
		#region Delete User
		[HttpDelete("{id}")]
		public IActionResult DeleteUser(int id)
		{
			var isDeleted = _userRepository.Delete(id);
			if (!isDeleted)
			{
				return NotFound();
			}
			return NoContent();
		}
		#endregion
	}
}
