using System;
using System.Collections.Generic;
using System.Text;

namespace GetTripSystem.Interfaces
{
    public interface ICreateOperation
    {
        Task AddPicture(int tripID, string filePath);
        Task RegisterTrip(int id, string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact);
    }
}
