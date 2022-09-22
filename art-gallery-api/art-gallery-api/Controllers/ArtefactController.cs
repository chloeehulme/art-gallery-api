using System;
using art_gallery_api.Models;
using art_gallery_api.Persistence;
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

        [HttpGet()]
        public IEnumerable<Artefact> GetAllArtefacts() =>
            _artefactRepo.GetArtefacts();

        [HttpGet("{id}", Name = "GetArtefact")]
        public IActionResult GetStateById(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();
            else return Ok(artefact);
        }

        [HttpPut("{id}")]
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

        [HttpPost()]
        public IActionResult AddArtefact(Artefact newArtefact)
        {
            if (newArtefact is null) return BadRequest();

            if (_artefactRepo.GetArtefacts()
                .Exists(x => x.Title.ToLower() == newArtefact.Title.ToLower()
                && x.Artist.ToLower() == newArtefact.Artist.ToLower())) return Conflict();

            _artefactRepo.AddArtefact(newArtefact);

            return CreatedAtRoute("GetArtefact", new { id = newArtefact.Id }, newArtefact);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteArtefact(int id)
        {
            Artefact? artefact = _artefactRepo.GetArtefactById(id);
            if (artefact is null) return NotFound();

            _artefactRepo.DeleteArtefact(id);
            return NoContent();
        }
    }
}

