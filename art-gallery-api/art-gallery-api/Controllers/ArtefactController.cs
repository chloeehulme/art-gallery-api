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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetAllArtefacts() =>
            _artefactRepo.GetArtefacts();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetArtefact")]
        [AllowAnonymous]
        public IActionResult GetArtefactById(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();
            else return Ok(artefact);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artistid"></param>
        /// <param name="updatedArtefact"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="artistid"></param>
        /// <param name="newArtefact"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteArtefact(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();

            _artefactRepo.DeleteArtefact(id);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet("state/{state}", Name = "GetArtefactByState")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactByState(string state) =>
            _artefactRepo.GetArtefactByState(state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("language/{language}", Name = "GetArtefactByLanguage")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactsByLanguage(string language) =>
            _artefactRepo.GetArtefactsByLanguage(language);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        [HttpGet("artist/{artist}", Name = "GetArtefactByArtist")]
        [AllowAnonymous]
        public IEnumerable<Artefact> GetArtefactsByArtist(string artist) =>
            _artefactRepo.GetArtefactsByArtist(artist);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("new")]
        [AllowAnonymous]
        public int GetRecentArtefactCount() =>
            _artefactRepo.GetRecentArtefactCount();
    }
}

