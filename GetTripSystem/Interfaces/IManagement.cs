using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface IManagement
    {
        Task<List<string>> GetMembersOfTrip(int tripId);
        Task KickMember(int id);
    }
}
