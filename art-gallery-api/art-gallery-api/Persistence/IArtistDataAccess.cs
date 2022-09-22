using System;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IArtistDataAccess
    {
        List<Artist> GetArtists();

        Artist? GetArtistById(int id);

        void UpdateArtist(int id, Artist updatedArtist);

        void AddArtist(Artist newArtist);

        void DeleteArtist(int id);
    }
}

