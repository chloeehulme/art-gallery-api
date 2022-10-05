using System;
using System.Globalization;
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

        // Gets all artists from DB 
        public List<Artist> GetArtists()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var artists = conn.Query<Artist>(command).AsList();
            return artists;
        }

        // Gets artist by Id from DB
        public Artist? GetArtistById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var artist = conn.QueryFirstOrDefault<Artist>(command);
            return artist;
        }

        // Updates artist record in DB
        public void UpdateArtist(int id, Artist updatedArtist)
        {
            string command = $"UPDATE {TABLE_NAME} SET name=@name, description=@description, " +
                $"age=@age, state_id=@state_id, language_group=@language_group, " +
                $"modified_date=@modified_date WHERE id={id}";

            var queryArgs = new
            {
                name = updatedArtist.Name,
                description = updatedArtist.Description ?? (object)DBNull.Value,
                age = updatedArtist.Age,
                state_id = updatedArtist.StateId,
                language_group = updatedArtist.LanguageGroup,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Adds artist record to DB
        public void AddArtist(int state_id, Artist newArtist)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @name, @description, " +
                $"@age, @state_id, @language_group, @created_date, @modified_date)";

            var queryArgs = new
            {
                name = newArtist.Name,
                description = newArtist.Description,
                age = newArtist.Age,
                state_id,
                language_group = newArtist.LanguageGroup,
                created_date = DateTime.Now,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Deletes artist record from DB
        public void DeleteArtist(int id)
        {
            string command = $"DELETE FROM {TABLE_NAME} WHERE id={id}";
            conn.Query(command);
        }

        // Gets artists by state_id field from DB
        public List<Artist> GetArtistsByState(string state)
        {
            string fstate = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(state);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE state_id=(SELECT id FROM public.state " +
                $"WHERE name='{fstate}');";

            var artists = conn.Query<Artist>(command).AsList();
            return artists;
        }

        // Gets artists by language_group field from DB
        public IEnumerable<Artist> GetArtistsByLanguage(string language)
        {
            string flanguage = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(language);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE language_group='{flanguage}'";

            var artists = conn.Query<Artist>(command).AsList();
            return artists;
        }

        // Gets artists by artefact title in DB
        public IEnumerable<Artist> GetArtistsByArtefact(string title)
        {
            string ftitle = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(title);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE id=(SELECT artist_id FROM public.artefact " +
                $"WHERE title='{ftitle}');";

            var artists = conn.Query<Artist>(command).AsList();
            return artists;
        }

        // Gets the number of artists from Victoria
        public int GetVictorianArtistCount()
        {
            string command = "SELECT get_artist_count_by_state(1)";

            int count = conn.QueryFirstOrDefault<int>(command);
            return count;
        }
    }
}

