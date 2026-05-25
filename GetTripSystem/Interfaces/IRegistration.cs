using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface IRegistration
    {
        Task AddMember(int userID, int tripID);
        Task<List<Trip>> GetAllTrips();
        Task<Trip?> GetTrip(int userID, int tripID);
        Task<List<string>> GetPictures(int tripID);
    }
}
