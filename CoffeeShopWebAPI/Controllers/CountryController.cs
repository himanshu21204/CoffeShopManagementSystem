using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryRepository _countryRepository;

        public CountryController(CountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        #region Get All Countrys
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            var countries = _countryRepository.SelectAll();
            return Ok(countries);
        }
        #endregion
        #region Get Country BY ID
        [HttpGet("{id}")]
        public IActionResult GetCountryByID(int id)
        {
            var country = _countryRepository.SelectByPK(id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
        #endregion
        #region Insert Country
        [HttpPost]
        public IActionResult InsertCountry([FromBody] CountryModel country)
        {
            if (country == null) return BadRequest();

            bool isInserted = _countryRepository.InsertCountry(country);

            if (isInserted) return Ok(new { Message = "Country Inserted Successfully" });

            return StatusCode(500, "Error occurred");
        }
        #endregion
        #region Update Country
        [HttpPut("{id}")]
        public IActionResult UpdateCountry(int id, [FromBody] CountryModel country)
        {
            if (country == null || id != country.CountryID) return BadRequest();

            bool isUpdated = _countryRepository.UpdateCountry(country);

            if (!isUpdated) return NotFound();

            return NoContent();
        }
        #endregion
        #region Delete Country By ID
        [HttpDelete("{id}")]
        public IActionResult DeleteCountryByID(int id)
        {
            var isDeleted = _countryRepository.DeleteCountry(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        #endregion
    }
}
