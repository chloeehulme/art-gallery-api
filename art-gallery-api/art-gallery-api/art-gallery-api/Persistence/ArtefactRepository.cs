using System;
using art_gallery_api.Models;
using Npgsql;
using Dapper;
using System.Globalization;

namespace art_gallery_api.Persistence
{
    public class ArtefactRepository : IArtefactDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=ARTGALLERY";

        private const string TABLE_NAME = "public.artefact";
        private NpgsqlConnection conn;

        public ArtefactRepository()
        {
            conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
        }

        // Gets all artefacts from DB
        public List<Artefact> GetArtefacts()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var artefact = conn.Query<Artefact>(command).AsList();
            return artefact;
        }

        // Gets artefact by Id from DB
        public Artefact? GetArtefactById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var artefact = conn.QueryFirstOrDefault<Artefact>(command);
            return artefact;
        }

        // Updates artefact record in DB
        void IArtefactDataAccess.UpdateArtefact(int id, Artefact updatedArtefact)
        {
            string command = $"UPDATE {TABLE_NAME} SET title=@title, description=@description, " +
                $"medium=@medium, year=@year, height_cm=@height_cm, width_cm=@width_cm, " +
                $"img_url=@img_url, artist_id=@artist_id, modified_date=@modified_date " +
                $"WHERE id={id}";

            var queryArgs = new
            {
                title = updatedArtefact.Title,
                description = updatedArtefact.Description ?? (object)DBNull.Value,
                medium = updatedArtefact.Medium,
                year = updatedArtefact.Year,
                height_cm = updatedArtefact.HeightCm,
                width_cm = updatedArtefact.WidthCm,
                img_url = updatedArtefact.ImgUrl ?? (object)DBNull.Value,
                artist_id = updatedArtefact.ArtistId,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Adds artefact record to DB
        void IArtefactDataAccess.AddArtefact(int artist_id, Artefact newArtefact)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @artist_id, @title, " +
                $"@description, @medium, @year, @height_cm, @width_cm, @img_url, @created_date, " +
                $"@modified_date)";

            var queryArgs = new
            {
                artist_id,
                title = newArtefact.Title,
                description = newArtefact.Description ?? (object)DBNull.Value,
                medium = newArtefact.Medium,
                year = newArtefact.Year,
                height_cm = newArtefact.HeightCm,
                width_cm = newArtefact.WidthCm,
                img_url = newArtefact.ImgUrl ?? (object)DBNull.Value,
                created_date = DateTime.Now,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Deletes artefact record from DB
        void IArtefactDataAccess.DeleteArtefact(int id)
        {
            string command = $"DELETE FROM {TABLE_NAME} WHERE id={id}";
            conn.Query(command);
        }

        // Gets artefact by artist's state of origin from DB
        public IEnumerable<Artefact> GetArtefactByState(string state)
        {
            string fstate = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(state);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE artist_id=(SELECT id FROM public.artist " +
                $"WHERE state_id=(SELECT id FROM public.state WHERE name='{fstate}'))";

            var artefacts = conn.Query<Artefact>(command).AsList();
            return artefacts;
        }

        // Gets artefact by artist's language group from DB
        public IEnumerable<Artefact> GetArtefactsByLanguage(string language)
        {
            string flanguage = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(language);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE artist_id=(SELECT id FROM public.artist " +
                $"WHERE language_group='{flanguage}')";

            var artefacts = conn.Query<Artefact>(command).AsList();
            return artefacts;
        }

        // Gets artefact by artist
        public IEnumerable<Artefact> GetArtefactsByArtist(string artist)
        {
            string fartist = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(artist);

            string command = $"SELECT * FROM {TABLE_NAME} " +
                $"WHERE artist_id=(SELECT id FROM public.artist " +
                $"WHERE name='{fartist}')";

            var artefacts = conn.Query<Artefact>(command).AsList();
            return artefacts;
        }

        // Gets the number of artefacts made within the past 5 years
        public int GetRecentArtefactCount()
        {
            DateTime currentdate = DateTime.Now;
            int yearfrom = currentdate.Year - 5;
            int yearto = currentdate.Year;

            string command = $"SELECT get_artefact_count({yearfrom}, {yearto});";

            int count = conn.QueryFirstOrDefault<int>(command);
            return count;
        }
    }
}

