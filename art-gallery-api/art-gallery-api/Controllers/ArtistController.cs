using System;
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

        [HttpGet()]
        public IEnumerable<Artist> GetAllArtists() =>
            _artistRepo.GetArtists();

        [HttpGet("{id}", Name = "GetArtist")]
        public IActionResult GetStateById(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();
            else return Ok(artist);
        }

        [HttpPut("{stateid}/{id}")]
        public IActionResult UpdateArtist(int id, int stateid, Artist updatedArtist)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();

            try
            {
                _artistRepo.UpdateArtist(id, stateid, updatedArtist);
            }
            catch (Exception er) { return BadRequest(er); }

            return NoContent();
        }

        [HttpPost("{stateid}")]
        public IActionResult AddArtist(int stateid, Artist newArtist)
        {
            if (newArtist is null) return BadRequest();

            if (_artistRepo.GetArtists()
                .Exists(x => x.Name.ToLower() == newArtist.Name.ToLower())) return Conflict();

            _artistRepo.AddArtist(stateid, newArtist);

            return CreatedAtRoute("GetArtist", new { id = newArtist.Id }, newArtist);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArtist(int id)
        {
            Artist? artist = _artistRepo.GetArtistById(id);
            if (artist is null) return NotFound();

            _artistRepo.DeleteArtist(id);
            return NoContent();
        }

        // GET artist by state name (split string at % -> new%south%wales)

        // GET artist by language group name

        // GET artist by age

        // GET using postgres funtion
    }
}

