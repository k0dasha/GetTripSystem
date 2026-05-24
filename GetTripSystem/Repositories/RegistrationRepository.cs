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
        public async Task Add(int userID, int tripID)
        {
            var reg = new Registration
            {
                UserID = userID,
                TripID = tripID,
                UserStatus = "active"
            };
            await _context.AddAsync(reg);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMember(int id, string status)
        {
            await _context.Registrations
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.UserStatus, status));
            //await _context.SaveChangesAsync();
        }
        public async Task<List<Registration>> GetUsersRegistrations (int userId) //где юзер статус не равен kicked или left
        {
            return await _context.Registrations.Where(c => c.UserID == userId && c.UserStatus == "active").ToListAsync();
        }
        public async Task<List<int>> GetMembersOfTrip(int tripId)
        {
            return await _context.Registrations
                .Where(r => r.TripID == tripId && r.UserStatus == "active")
                .Select(r => r.UserID)
                .ToListAsync();
        }
        public async Task<string?> GetUserStatus(int userId, int tripId)
        {
            return await _context.Registrations
                .Where(r => r.UserID == userId && r.TripID == tripId)
                .Select(r => r.UserStatus)
                .FirstOrDefaultAsync();
        }
        public async Task<int> GetCurrentMembersCount(int tripID)
        {
            return await _context.Registrations.CountAsync(u => u.TripID == tripID && u.UserStatus == "active");
        }
    }
}
