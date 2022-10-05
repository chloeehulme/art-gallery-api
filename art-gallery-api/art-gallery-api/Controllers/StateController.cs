using System;
using System.Globalization;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authorization;
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
        /// Gets all states.
        /// </summary>
        /// <returns> A list of all states. </returns>
        /// <response code="200"> Returns a list of all states, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        [AllowAnonymous]
        public IEnumerable<State> GetAllStatesAndTerritories() =>
            _stateRepo.GetStates();


        /// <summary>
        /// Gets a state with the specified id.
        /// </summary>
        /// <param name="id"> The id for the state we are looking to return (from the request URL).</param>
        /// <returns> A state with the specified id.</returns>
        /// <response code="200"> Returns the state with the specified id. </response>
        /// <response code="404"> If the state retrieved at the specified id is null,
        /// ie. the state does not exist. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetStateById")]
        [AllowAnonymous]
        public IActionResult GetStateById(int id)
        {
            State? state = _stateRepo.GetStateById(id);
            if (state is null) return NotFound();
            else return Ok(state);
        }

        /// <summary>
        /// Updates an existing state.
        /// </summary>
        /// <param name="id"> The id for the state we are looking to update (from the request URL). </param>
        /// <param name="updatedState"> Updated details for the state (from the HTTP request body). </param>
        /// <returns> No content. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     PUT /api/art-gallery/states/{id}
        ///     {
        ///         "name": "Victoria,
        ///         "languageGroups": 38
        ///     }
        ///
        /// </remarks>
        /// <response code="204"> If the state is udpated successfully. </response>
        /// <response code="400"> If the request body is null. </response>
        /// <response code="404"> If the state to update is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
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

