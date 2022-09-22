using System;
using art_gallery_api.Models;
using Npgsql;
using Dapper;

namespace art_gallery_api.Persistence
{
    public class StateRepository :IStateDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=ARTGALLERY";

        private const string TABLE_NAME = "public.state";
        private NpgsqlConnection conn;

        public StateRepository()
        {
            conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
        }

        public List<State> GetStates()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var states = conn.Query<State>(command).ToList();
            return states.ToList();
        }

        public State? GetStateById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var state = conn.QueryFirstOrDefault<State>(command);
            return state;
        }

        public void UpdateState(int id, State updatedState)
        {
            string command = $"UPDATE {TABLE_NAME} SET name=@name, language_groups=@language_groups," +
                $"modified_date=@modified_date WHERE id={id} RETURNING *";

            var queryArgs = new
            {
                name = updatedState.Name,
                language_groups = updatedState.LanguageGroups,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // TODO: Implement when Western Australia finally leaves us...
        public void DeleteState(int id)
        {
            throw new NotImplementedException();
        }
    }
}

