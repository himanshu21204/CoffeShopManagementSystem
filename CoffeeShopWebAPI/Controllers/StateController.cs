using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly StateRepository _stateRepository;
        public StateController(StateRepository stateRepository)
        {
            this._stateRepository = stateRepository;
        }
        #region Get All States
        [HttpGet]
        public IActionResult GetAllStates()
        {
            var states = _stateRepository.StateGetAll();
            return Ok(states);
        }
        #endregion
        #region Get State By ID
        [HttpGet("{id}")]
        public IActionResult GetStateByID(int id)
        {
            var state = _stateRepository.SelectByID(id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }
        #endregion
        #region Insert State
        [HttpPost]
        public IActionResult InsertState([FromBody] StateModel state)
        {
            if (state == null) return BadRequest();

            bool isInserted = _stateRepository.InsertState(state);

            if (isInserted) return Ok(new { Message = "State Inserted Successfully" });

            return StatusCode(500, "Error occurred");
        }
        #endregion
        #region Update State
        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, [FromBody] StateModel state)
        {
            if (state == null || id != state.StateID) return BadRequest();

            bool isUpdated = _stateRepository.UpdateState(state);

            if (!isUpdated) return NotFound();

            return NoContent();
        }
        #endregion
        #region Delete State By ID
        [HttpDelete("{id}")]
        public IActionResult DeleteStateByID(int id)
        {
            var isDeleted = _stateRepository.DeleteState(id);
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
            var countrys = _stateRepository.CountryDropDown();
            return Ok(countrys);
        }
        #endregion
    }
}
