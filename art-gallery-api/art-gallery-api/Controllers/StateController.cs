using System;
using System.Globalization;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/art-gallery/states")]
    public class StateController : ControllerBase
    {
        private readonly IStateDataAccess _stateRepo;
        public StateController(IStateDataAccess stateRepo)
        {
            _stateRepo = stateRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IEnumerable<State> GetAllStatesAndTerritories() =>
            _stateRepo.GetStates();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetStateById")]
        public IActionResult GetStateById(int id)
        {
            State? state = _stateRepo.GetStateById(id);
            if (state is null) return NotFound();
            else return Ok(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedState"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, State updatedState)
        {
            State? state = _stateRepo.GetStateById(id);
            if (state is null) return NotFound();

            try
            {
                _stateRepo.UpdateState(id, updatedState);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }
    }
}

