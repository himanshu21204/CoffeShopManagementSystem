using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class DashboardController : ControllerBase
	{
		private readonly DashboardRepository _dashboardRepository;

		public DashboardController(DashboardRepository dashboardRepository)
		{
			_dashboardRepository = dashboardRepository;
		}

		#region Get All Dashboard Data
		[HttpGet]
		public async Task<IActionResult> GetAllDashboard()
		{
			var dashboards = await _dashboardRepository.Index();
			return Ok(dashboards);
		}
		#endregion
	}
}
