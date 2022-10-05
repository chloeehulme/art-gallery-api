using System;
using System.Globalization;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace art_gallery_api.Controllers
{
    [ApiController]
    [Route("api/art-gallery/artists")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistDataAccess _artistRepo;
        public ArtistController(IArtistDataAccess artistRepo)
        {
            _artistRepo = artistRepo;
        }

        /// <summary>
        /// Gets all artists.
        /// </summary>
        /// <returns> A list of all artists. </returns>
        /// <response code="200"> Returns a list of all artists, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet()]
        [AllowAnonymous]
        public IEnumerable<Artist> GetAllArtists() =>
            _artistRepo.GetArtists();

        /// <summary>
        /// Gets an artist with the specified id.
        /// </summary>
        /// <param name="id"> The id for the artist we are looking to return (from the request URL).</param>
        /// <returns> An artist with the specified id.</returns>
        /// <response code="200"> Returns the artist with the specified id. </response>
        /// <response code="404"> If the artist retrieved at the specified id is null,
        /// ie. the artist does not exist. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}", Name = "GetArtist")]
        [AllowAnonymous]
        public IActionResult GetArtistById(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();
            else return Ok(artist);
        }

        /// <summary>
        /// Updates an existing artist.
        /// </summary>
        /// <param name="id"> The id for the artist we are looking to update (from the request URL). </param>
        /// <param name="updatedArtist"> Updated details for the artist (from the HTTP request body). </param>
        /// <returns> No content. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     PUT /api/art-gallery/artists/{id}
        ///     {
        ///         "name": "John Doe",
        ///         "age": 22,
        ///         "stateId": 5,
        ///         "languageGroup": "Woiwurrung"
        ///     }
        ///
        /// </remarks>
        /// <response code="204"> If the artist is udpated successfully. </response>
        /// <response code="400"> If the request body is null. </response>
        /// <response code="404"> If the artist to update is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateArtist(int id, Artist updatedArtist)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();

            try
            {
                _artistRepo.UpdateArtist(id, updatedArtist);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }

        /// <summary>
        /// Creates a new artist.
        /// </summary>
        /// <param name="stateid"> The id for the state that the artist is from. </param>
        /// <param name="newArtist"> A new artist (from the HTTP request body). </param>
        /// <returns> A newly created artist. </returns>
        /// <remarks>
        /// Sample request body:
        ///
        ///     POST /api/art-gallery/state/2/artist
        ///     {
        ///         "name": "Mark Smith",
        ///         "age": 46,
        ///         "languageGroup": "Woiwurrung"
        ///     }
        ///
        /// </remarks>
        /// <response code="201"> Returns a newly created artist. </response>
        /// <response code="400"> If the new artist is null. </response>
        /// <response code="409"> If an artist with the same name already exists. </response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("state/{stateid}/artist"), Authorize(Policy = "AdminOnly")]
        public IActionResult AddArtist(int stateid, Artist newArtist)
        {
            if (newArtist is null) return BadRequest();

            if (_artistRepo.GetArtists()
                .Exists(x => x.Name.ToLower() == newArtist.Name.ToLower())) return Conflict();

            _artistRepo.AddArtist(stateid, newArtist);

            return CreatedAtRoute("GetArtist", new { id = newArtist.Id }, newArtist);
        }

        /// <summary>
        /// Deletes an artist with the specified id.
        /// </summary>
        /// <param name="id"> The id for the artist we are looking to delete (from the request URL). </param>
        /// <returns> No content. </returns>
        /// <response code="204"> If the artist at the secified id is successfully deleted. </response>
        /// <response code="404"> If the artist to delete is null. </response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteArtist(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();

            _artistRepo.DeleteArtist(id);
            return NoContent();
        }


        /// <summary>
        /// Gets all artists from the specified state.
        /// </summary>
        /// <param name="state"> The state of the artists we are looking to return (from the request URL). </param>
        /// <returns> A list of all artists from the specified. </returns>
        /// <response code="200"> Returns a list of artists from the specified, or an empty
        /// list if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("state/{state}", Name = "GetArtistByState")]
        [AllowAnonymous]
        public IEnumerable<Artist> GetArtistsByState(string state) =>
            _artistRepo.GetArtistsByState(state);

        /// <summary>
        /// Gets all artists from the specified language group.
        /// </summary>
        /// <param name="language"> The language group of the artists we are looking to return (from the request URL). </param>
        /// <returns> A list of artists from the specified language group. </returns>
        /// <response code="200"> Returns a list of artists from the specified language group, or an empty
        /// list if there are currently none stored. </response>
        [HttpGet("language/{language}", Name = "GetArtistByLanguage")]
        [AllowAnonymous]
        public IEnumerable<Artist> GetArtistsByLanguage(string language) =>
            _artistRepo.GetArtistsByLanguage(language);

        /// <summary>
        /// Gets all artists who have created a specific artefact.
        /// </summary>
        /// <param name="title"> The title of the artefact created by the artist we are looking to return (from the request URL). </param>
        /// <returns> A list of artists who have contributed to the creation of the specified artefact. </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("artefact/{title}", Name = "GetArtistByArtefact")]
        [AllowAnonymous]
        public IEnumerable<Artist> GetArtistsByArtefact(string title) =>
            _artistRepo.GetArtistsByArtefact(title);

        /// <summary>
        /// Gets the number of artists from Victoria.
        /// </summary>
        /// <returns> The number of artists from Victoria. </returns>
        /// <response code="200"> Returns the number of artists from Victoria, or 0
        /// if there are currently none stored. </response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("victorian")]
        [AllowAnonymous]
        public int GetVictorianArtistCount() =>
            _artistRepo.GetVictorianArtistCount();
    }
}

