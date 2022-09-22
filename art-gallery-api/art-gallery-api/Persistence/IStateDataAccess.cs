using System;
using art_gallery_api.Models;

namespace art_gallery_api.Persistence
{
    public interface IStateDataAccess
    {
        List<State> GetStates();

        State? GetStateById(int id);

        void UpdateState(int id, State updatedState);

        void DeleteState(int id);
    }
}