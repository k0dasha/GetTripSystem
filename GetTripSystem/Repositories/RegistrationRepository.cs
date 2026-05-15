using GetTripSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Repositories
{
    public class RegistrationRepository
    {
        private readonly Context _context;
        public RegistrationRepository(Context context)
        {
            _context = context;
        }
        public async Task Add(int id, int userID, int tripID, string status)
        {
            var reg = new Registration
            {
                Id = id,
                UserID = userID,
                TripID = tripID,
                UserStatus = "active"
            };
            await _context.AddAsync(reg);
            await _context.SaveChangesAsync();
        }
        public async Task Update(int id, string status)
        {
            await _context.Registrations
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.UserStatus, status));
            await _context.SaveChangesAsync();
        }
        public async Task<List<Registration>> GetUsersRegistrations (int userId)
        {
            return await _context.Registrations.Where(c => c.UserID == userId).ToListAsync();
        }
        public async Task<List<Registration>> GetMembersOfTrip(int tripId)
        {
            return await _context.Registrations.Where(c => c.TripID == tripId).ToListAsync();
        }
    }
}
