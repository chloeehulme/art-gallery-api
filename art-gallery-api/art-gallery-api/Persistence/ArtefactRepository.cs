using System;
using art_gallery_api.Models;
using Npgsql;
using Dapper;

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

        public List<Artefact> GetArtefacts()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var artefact = conn.Query<Artefact>(command).ToList();
            return artefact.ToList();
        }

        public Artefact? GetArtefactById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var artefact = conn.QueryFirstOrDefault<Artefact>(command);
            return artefact;
        }

        void IArtefactDataAccess.UpdateArtefact(int id, Artefact updatedArtefact)
        {
            string command = $"UPDATE {TABLE_NAME} SET title=@title, description=@description, medium=@medium, " +
                $"year=@year, height_cm=@height_cm, width_cm=@width_cm, img_url=@img_url, artist=@artist, " +
                $"modified_date=@modified_date WHERE id={id} RETURNING *";

            var queryArgs = new
            {
                title = updatedArtefact.Title,
                description = updatedArtefact.Description ?? (object)DBNull.Value,
                medium = updatedArtefact.Medium,
                year = updatedArtefact.Year,
                height_cm = updatedArtefact.HeightCm,
                width_cm = updatedArtefact.WidthCm,
                img_url = updatedArtefact.ImgUrl ?? (object)DBNull.Value,
                artist = updatedArtefact.Artist,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        void IArtefactDataAccess.AddArtefact(Artefact newArtefact)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @title, @description, @medium, @year, @height_cm," +
                $" @width_cm, @img_url, @artist, @created_date @modified_date)";

            var queryArgs = new
            {
                title = newArtefact.Title,
                description = newArtefact.Description ?? (object)DBNull.Value,
                medium = newArtefact.Medium,
                year = newArtefact.Year,
                height_cm = newArtefact.HeightCm,
                width_cm = newArtefact.WidthCm,
                img_url = newArtefact.ImgUrl ?? (object)DBNull.Value,
                artist = newArtefact.Artist,
                created_date = DateTime.Now,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        void IArtefactDataAccess.DeleteArtefact(int id)
        {
            string command = $"DELETE * FROM {TABLE_NAME} WHERE id={id}";
            conn.Query(command);
        }
    }
}

