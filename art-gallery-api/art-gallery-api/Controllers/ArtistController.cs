using System;
using System.Globalization;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IEnumerable<Artist> GetAllArtists() =>
            _artistRepo.GetArtists();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetArtist")]
        public IActionResult GetArtistById(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();
            else return Ok(artist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stateid"></param>
        /// <param name="updatedArtist"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
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
        /// 
        /// </summary>
        /// <param name="stateid"></param>
        /// <param name="newArtist"></param>
        /// <returns></returns>
        [HttpPost("state/{stateid}/artist")]
        public IActionResult AddArtist(int stateid, Artist newArtist)
        {
            if (newArtist is null) return BadRequest();

            if (_artistRepo.GetArtists()
                .Exists(x => x.Name.ToLower() == newArtist.Name.ToLower())) return Conflict();

            _artistRepo.AddArtist(stateid, newArtist);

            return CreatedAtRoute("GetArtist", new { id = newArtist.Id }, newArtist);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();

            _artistRepo.DeleteArtist(id);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet("state/{state}", Name = "GetArtistByState")]
        public IEnumerable<Artist> GetArtistsByState(string state) =>
            _artistRepo.GetArtistsByState(state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("language/{language}", Name = "GetArtistByLanguage")]
        public IEnumerable<Artist> GetArtistsByLanguage(string language) =>
            _artistRepo.GetArtistsByLanguage(language);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("artefact/{title}", Name = "GetArtistByArtefact")]
        public IEnumerable<Artist> GetArtistsByArtefact(string title) =>
            _artistRepo.GetArtistsByArtefact(title);


        // GET artist by age

        // GET using postgres funtion
    }
}

