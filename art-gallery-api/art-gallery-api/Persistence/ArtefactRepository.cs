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

            var artefact = conn.Query<Artefact>(command).AsList();
            return artefact;
        }

        public Artefact? GetArtefactById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var artefact = conn.QueryFirstOrDefault<Artefact>(command);
            return artefact;
        }

        void IArtefactDataAccess.UpdateArtefact(int id, int artistid, Artefact updatedArtefact)
        {
            string command = $"UPDATE {TABLE_NAME} SET title=@title, description=@description, medium=@medium, " +
                $"year=@year, heightcm=@heightcm, widthcm=@widthcm, imgurl=@imgurl, artistid={artistid}, " +
                $"modifieddate=@modifieddate WHERE id={id} RETURNING *";

            var queryArgs = new
            {
                title = updatedArtefact.Title,
                description = updatedArtefact.Description ?? (object)DBNull.Value,
                medium = updatedArtefact.Medium,
                year = updatedArtefact.Year,
                heightcm = updatedArtefact.HeightCm,
                widthcm = updatedArtefact.WidthCm,
                imgurl = updatedArtefact.ImgUrl ?? (object)DBNull.Value,
                modifieddate = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        void IArtefactDataAccess.AddArtefact(int artistid, Artefact newArtefact)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @title, @description, @medium, @year, @heightcm," +
                $" @widthcm, @imgurl, @artistid, @createddate @modifieddate)";

            var queryArgs = new
            {
                title = newArtefact.Title,
                description = newArtefact.Description ?? (object)DBNull.Value,
                medium = newArtefact.Medium,
                year = newArtefact.Year,
                heightcm = newArtefact.HeightCm,
                widthcm = newArtefact.WidthCm,
                imgurl = newArtefact.ImgUrl ?? (object)DBNull.Value,
                artistid,
                createddate = DateTime.Now,
                modifieddate = DateTime.Now
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

