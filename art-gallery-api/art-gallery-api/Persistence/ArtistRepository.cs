using System;
using art_gallery_api.Models;
using Dapper;
using Npgsql;

namespace art_gallery_api.Persistence
{
    public class ArtistRepository : IArtistDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=ARTGALLERY";

        private const string TABLE_NAME = "public.artist";
        private NpgsqlConnection conn;

        public ArtistRepository()
        {
            conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
        }

        public List<Artist> GetArtists()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var artists = conn.Query<Artist>(command).AsList();
            return artists;
        }

        public Artist? GetArtistById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var artist = conn.QueryFirstOrDefault<Artist>(command);
            return artist;
        }

        public void UpdateArtist(int id, Artist updatedArtist)
        {
            string command = $"UPDATE {TABLE_NAME} SET name=@name, description=@description, age=@age, state=@state" +
                $"languagegroup=@languagegroup, modifieddate=@modifieddate WHERE id={id}";

            var queryArgs = new
            {
                name = updatedArtist.Name,
                description = updatedArtist.Description ?? (object)DBNull.Value,
                age = updatedArtist.Age,
                state = updatedArtist.State,
                languagegroup = updatedArtist.LanguageGroup,
                modifieddate = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        public void AddArtist(Artist newArtist)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @name, @description, @age, @state, @languagegroup," +
                $" @createddate, @modifieddate)";

            var queryArgs = new
            {
                name = newArtist.Name,
                description = newArtist.Description,
                age = newArtist.Age,
                state = newArtist.State,
                languagegroup = newArtist.LanguageGroup,
                createddate = DateTime.Now,
                modifieddate = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        public void DeleteArtist(int id)
        {
            string command = $"DELETE * FROM {TABLE_NAME} WHERE id={id}";
            conn.Query(command);
        }
    }
}

