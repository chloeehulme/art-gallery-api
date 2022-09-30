using System;
using Npgsql;
using art_gallery_api.Models;
using Microsoft.AspNetCore.Identity;
using Dapper;

namespace art_gallery_api.Persistence
{
    public class UserRepository : IUserDataAccesss
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=ARTGALLERY";

        private const string TABLE_NAME = "public.user";
        private NpgsqlConnection conn;

        public UserRepository()
        {
            conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
        }

        // Gets all users from DB
        public List<User> GetUsers()
        {
            string command = $"SELECT * FROM {TABLE_NAME}";

            var users = conn.Query<User>(command).AsList();
            return users;
        }

        // Gets all admin users from DB
        public List<User> GetAdminUsers()
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE role='admin'";

            var users = conn.Query<User>(command).AsList();
            return users;
        }

        // Gets user by id from DB
        public User? GetUserById(int id)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE id={id}";

            var user = conn.QueryFirstOrDefault<User>(command);
            return user;
        }

        // Adds user to DB
        public void AddUser(User newUser)
        {
            string command = $"INSERT INTO {TABLE_NAME} VALUES(DEFAULT, @email, @first_name, @last_name, " +
                    "@password_hash, @description, @role, @created_date, @modified_date)";

            var password = newUser.PasswordHash;
            var user = newUser;
            var hasher = new PasswordHasher<User>();
            var pwHash = hasher.HashPassword(user, password);
            user.PasswordHash = pwHash;

            var queryArgs = new
            {
                email = newUser.Email,
                first_name = newUser.FirstName,
                last_name = newUser.LastName,
                password_hash = newUser.PasswordHash,
                description = newUser.Description,
                role = newUser.Role,
                created_date = DateTime.Now,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Updates user details in DB
        public void UpdateUser(int id, User updatedUser)
        {
            var user = GetUserById(id);

            string command = $"UPDATE {TABLE_NAME} SET email=@email, password_hash=@password_hash, " +
                "first_name=@first_name, last_name=@last_name, description=@description, role=@role, " +
                $"modified_date=@modified_date WHERE id={id}";

            var queryArgs = new
            {
                email = user!.Email,
                password_hash = user.PasswordHash,
                first_name = updatedUser.FirstName,
                last_name = updatedUser.LastName,
                description = updatedUser.Description ?? (object)DBNull.Value,
                role = updatedUser.Role ?? (object)DBNull.Value,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Deletes user from DB
        public void DeleteUser(int id)
        {
            string command = $"DELETE * FROM {TABLE_NAME} WHERE id={id}";
            conn.Query(command);
        }

        // Patches email and password of user in DB
        public void PatchUser(int id, Login updatedLogin)
        {
            string command = $"UPDATE {TABLE_NAME} SET email=@email, password_hash=@password_hash," +
                $" modified_date=@modified_date WHERE id={id}";

            var password = updatedLogin.Password;
            var user = GetUserById(id)!;
            var hasher = new PasswordHasher<User>();
            var pwHash = hasher.HashPassword(user, password);

            var queryArgs = new
            {
                email = updatedLogin.Email,
                password_hash = pwHash,
                modified_date = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Gets user by email from DB
        public User? GetUserByEmail(string email)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE email='{email}'";

            var user = conn.QuerySingleOrDefault<User>(command);
            return user;
        }
    }
}

