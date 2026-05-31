using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface IManagement
    {
        Task<List<string>> GetMembersOfTrip(int tripId);
        Task<User?> GetUser(string userName, string passwd);
        Task KickMember(int id);
    }
}
