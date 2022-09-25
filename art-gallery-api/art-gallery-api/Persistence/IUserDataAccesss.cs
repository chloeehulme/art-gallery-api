using System;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IUserDataAccesss
    {
        List<User> GetUsers();

        List<User> GetAdminUsers();

        User? GetUserById(int id);

        void AddUser(User newUser);

        void UpdateUser(int id, User updatedUser);

        void DeleteUser(int id);

        void PatchUser(int id, Login updatedLogin);
    }
}

