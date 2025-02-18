using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityRepository _cityRepository;
        public CityController(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        #region Get All Citys
        [HttpGet]
        public IActionResult GetAllCities()
        {
            var cities = _cityRepository.SelectAll();
            return Ok(cities);
        }
        #endregion
        #region Get By ID City
        [HttpGet("{id}")]
        public IActionResult GetCityByID(int id)
        {
            var city = _cityRepository.SelectByPK(id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }
        #endregion
        #region Insert City
        [HttpPost]
        public IActionResult InsertCity([FromBody] CityModel city)
        {
            if (city == null) return BadRequest();

            bool isInserted = _cityRepository.Insert(city);

            if (isInserted) return Ok(new { Message = "City Inserted Successfully" });

            return StatusCode(500, "Error occurred");
        }
        #endregion
        #region Update City
        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, [FromBody] CityModel city)
        {
            if (city == null || id != city.CityID) return BadRequest();

            bool isInserted = _cityRepository.Update(city);

            if (!isInserted) return NotFound();

            return NoContent();
        }
        #endregion
        #region Delete City By ID
        [HttpDelete("{id}")]
        public IActionResult DeleteCityByID(int id)
        {
            var isDeleted = _cityRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion
        #region Country Drop Down
        [HttpGet]
        public IActionResult GetCountryDropDown()
        {
            var countrys = _cityRepository.CountryDropDown();
            return Ok(countrys);
        }
        #endregion
        #region State Drop Down By Using Country
        [HttpGet("{id}")]
        public IActionResult StateDropDown(int id)
        {
            var states = _cityRepository.StateDropDown(id);
            return states == null ? NotFound() : Ok(states);
        }
        #endregion
    }
}
