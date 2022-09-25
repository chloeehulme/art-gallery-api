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
            string command = $"SELECT* FROM {TABLE_NAME} WHERE role={"role"}";

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
            string command = $"INSERT INTO {TABLE_NAME} OUTPUT INSERTED.* VALUES(DEFAULT, @email, @firstname, @lastname, " +
                    "@passwordhash, @description, @role, @createddate, @modifieddate)";

            var password = newUser.PasswordHash;
            var user = newUser;
            var hasher = new PasswordHasher<User>();
            var pwHash = hasher.HashPassword(user, password);
            user.PasswordHash = pwHash;

            var queryArgs = new
            {
                email = newUser.Email,
                firstname = newUser.FirstName,
                lastname = newUser.LastName,
                passwordhash = newUser.PasswordHash,
                description = newUser.Description,
                role = newUser.Role,
                createddate = DateTime.Now,
                modifieddate = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Updates user details in DB
        public void UpdateUser(int id, User updatedUser)
        {
            var user = GetUserById(id);

            string command = $"UPDATE {TABLE_NAME} SET email=@email, passwordhash=@passwordhash, " +
                "firstname=@firstname, lastname=@lastname, description=@description, role=@role, " +
                $"modifieddate=@modifieddate WHERE id={id}";

            var queryArgs = new
            {
                email = user!.Email,
                passwordhash = user.PasswordHash,
                firstname = updatedUser.FirstName,
                lastname = updatedUser.LastName,
                description = updatedUser.Description ?? (object)DBNull.Value,
                role = updatedUser.Role ?? (object)DBNull.Value,
                modifieddate = DateTime.Now
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
            string command = $"UPDATE {TABLE_NAME} SET email=@email, passwordhash=@passwordhash," +
                $" modifieddate=@modifieddate WHERE id={id}";

            var password = updatedLogin.Password;
            var user = GetUserById(id)!;
            var hasher = new PasswordHasher<User>();
            var pwHash = hasher.HashPassword(user, password);

            var queryArgs = new
            {
                email = updatedLogin.Email,
                passwordhash = pwHash,
                modifieddate = DateTime.Now
            };

            conn.Execute(command, queryArgs);
        }

        // Gets user by email from DB
        public User? GetUserByEmail(string email)
        {
            string command = $"SELECT * FROM {TABLE_NAME} WHERE email={email}";

            var user = conn.QueryFirstOrDefault<User>(command);
            return user;
        }
    }
}

