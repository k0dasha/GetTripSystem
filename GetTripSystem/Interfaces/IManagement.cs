using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface IManagement
    {
        Task<List<User>> GetMembersOfTrip(int tripId);
        Task<User?> GetUser(string userName, string passwd);
        Task KickMember(int userId, int tripId);
        Task CancelRegistration(int userId, int tripId);
        Task<List<Trip>> GetUserRegistrations(int userId);
        Task<List<Trip>> GetUserTrips(int userId);
    }
}
