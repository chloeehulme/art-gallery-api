using System;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/art-gallery/artefacts")]
    public class ArtefactController : ControllerBase
    {
        private readonly IArtefactDataAccess _artefactRepo;
        public ArtefactController(IArtefactDataAccess artefactRepo)
        {
            _artefactRepo = artefactRepo;
        }

        /// <summary>
        /// Gets all artefacts
        /// </summary>
        /// <returns> A list of all artefacts. </returns>
        /// <response code="200"> Returns a list of all artefacts, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetAllArtefacts() =>
            _artefactRepo.GetArtefacts();

        /// <summary>
        /// Gets an artefact with the specified id.
        /// </summary>
        /// <param name="id"> The id for the artefact we are looking to return (from the request URL). </param>
        /// <returns> An artefact with the specified id. </returns>
        /// <response code="200"> Returns the artefact with the specified id. </response>
        /// <response code="404"> If the artefact retrieved at the specified id is null,
        /// ie. the artefact does not exist. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetArtefact")]
        [AllowAnonymous]
        public IActionResult GetArtefactById(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();
            else return Ok(artefact);
        }

        /// <summary>
        /// Updates an existing artefact.
        /// </summary>
        /// <param name="id"> The id for the artefact we are looking to update (from the request URL). </param>
        /// <param name="updatedArtefact"> Updated details for the artefact (from the HTTP request body). </param>
        /// <returns> No content. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     PUT /api/art-gallery/artefacts/{id}
        ///     {
        ///         "artistId": 1,
        ///         "title": "example",
        ///         "medium": "Acrylic on canvas",
        ///         "year": 2020,
        ///         "heightCm": 100,
        ///         "widthCm": 200
        ///     }
        ///
        /// </remarks>
        /// <response code="204"> If the artefact is udpated successfully. </response>
        /// <response code="400"> If the request body is null. </response>
        /// <response code="404"> If the artefact to update is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateArtefact(int id, Artefact updatedArtefact)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();

            try
            {
                _artefactRepo.UpdateArtefact(id, updatedArtefact);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }

        /// <summary>
        /// Creates a new artefact.
        /// </summary>
        /// <param name="artistid"> The id for the artist, whom the artefact is created by. </param>
        /// <param name="newArtefact"> A new artefact (from the HTTP request body). </param>
        /// <returns> A newly created artefact. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     POST /api/art-gallery/artist/4/artefact
        ///     {
        ///         "title": "example",
        ///         "medium": "Acrylic on canvas",
        ///         "year": 2022,
        ///         "heightCm": 150,
        ///         "widthCm": 100
        ///     }
        ///
        /// </remarks>
        /// <response code="201"> Returns a newly created artefact. </response>
        /// <response code="400"> If the new artefact is null. </response>
        /// <response code="409"> If an artefact with the same name, created by the same artist
        /// already exists. </response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("artist/{artistid}/artefact"), Authorize(Policy = "AdminOnly")]
        public IActionResult AddArtefact(int artistid, Artefact newArtefact)
        {
            if (newArtefact is null) return BadRequest();

            if (_artefactRepo.GetArtefacts()
                .Exists(x => x.Title.ToLower() == newArtefact.Title.ToLower()
                && x.ArtistId == artistid)) return Conflict();

            _artefactRepo.AddArtefact(artistid, newArtefact);

            return CreatedAtRoute("GetArtefact", new { id = newArtefact.Id }, newArtefact);
        }

        /// <summary>
        /// Deletes an artefact with the specified id.
        /// </summary>
        /// <param name="id"> The id for the artefact we are looking to delete (from the request URL). </param>
        /// <returns> No content. </returns>
        /// <response code="204"> If the artefact at the secified id is successfully deleted. </response>
        /// <response code="404"> If the artefact to delete is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteArtefact(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();

            _artefactRepo.DeleteArtefact(id);
            return NoContent();
        }

        /// <summary>
        /// Gets all artefacts made by artists from the specified state.
        /// </summary>
        /// <param name="state"> The state name for the artefact we are looking to return (from the request URL). </param>
        /// <returns> A list of artefacts created by artists from the specified state. </returns>
        /// <response code="200"> Returns a list of artefacts made by artists from the specified state, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("state/{state}", Name = "GetArtefactByState")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactByState(string state) =>
            _artefactRepo.GetArtefactByState(state);

        /// <summary>
        /// Gets all artefacts made by artists from the specified language group.
        /// </summary>
        /// <param name="language"> The language group of the artefacts we are looking to return (from the request URL). </param>
        /// <returns> A list of artefacts created by artists from the specified language group. </returns>
        /// <response code="200"> Returns a list of artefacts made by artists from the specified language group, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("language/{language}", Name = "GetArtefactByLanguage")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactsByLanguage(string language) =>
            _artefactRepo.GetArtefactsByLanguage(language);

        /// <summary>
        /// Gets all artefacts made by the specified artist.
        /// </summary>
        /// <param name="artist"> The artist who has made the artefacts we are looking to return (from the request URL). </param>
        /// <returns> A list of all artefacts created by the specified artist. </returns>
        /// <response code="200"> Returns a list of artefacts made by the specified artist, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("artist/{artist}", Name = "GetArtefactByArtist")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactsByArtist(string artist) =>
            _artefactRepo.GetArtefactsByArtist(artist);

        /// <summary>
        /// Gets the number of artefacts made within the past 5 years.
        /// </summary>
        /// <returns> The number of artefacts that have been made within the past 5 years. </returns>
        /// <response code="200"> Returns the number of artefacts that have been made within the past 5 years, or 0
        /// if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("new")]
        [AllowAnonymous]
        public int GetRecentArtefactCount() =>
            _artefactRepo.GetRecentArtefactCount();
    }
}

