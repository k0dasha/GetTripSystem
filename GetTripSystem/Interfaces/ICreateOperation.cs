using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface ICreateOperation
    {
        Task AddPicture(int tripID, string filePath);
        Task RegisterTrip(string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact);
        Task RegisterUser(string name, string password);
    }
}
