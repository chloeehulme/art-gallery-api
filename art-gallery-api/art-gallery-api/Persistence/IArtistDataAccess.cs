using System;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IArtistDataAccess
    {
        List<Artist> GetArtists();

        Artist? GetArtistById(int id);

        void UpdateArtist(int id, int stateid, Artist updatedArtist);

        void AddArtist(int stateid, Artist newArtist);

        void DeleteArtist(int id);
    }
}

