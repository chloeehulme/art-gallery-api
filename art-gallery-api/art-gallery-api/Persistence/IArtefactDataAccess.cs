using System;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IArtefactDataAccess
    {
        List<Artefact> GetArtefacts();

        Artefact? GetArtefactById(int id);

        void UpdateArtefact(int id, Artefact updatedArtefact);

        void AddArtefact(Artefact newArtefact);

        void DeleteArtefact(int id);
    }
}

