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
        public async Task UpdateMember(int tripId, string status, int userId)
        {
            await _context.Registrations
                .Where(x => x.TripID == tripId && x.UserID == userId)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.UserStatus, status));
        }
        public async Task<List<int>> GetMembersOfTrip(int tripId)
        {
            return await _context.Registrations
                .Where(r => r.TripID == tripId && r.UserStatus == "active")
                .Select(r => r.UserID)
                .ToListAsync();
        }
        public async Task<List<int>> GetUserRegs(int userId)
        {
            return await _context.Registrations
                .Where(r => r.UserID == userId && r.UserStatus == "active")
                .Select(r => r.TripID)
                .ToListAsync();
        }
        public async Task<string?> GetUserStatus(int userId, int tripId)
        {
            return await _context.Registrations
                .Where(r => r.UserID == userId && r.TripID == tripId)
                .Select(r => r.UserStatus)
                .LastOrDefaultAsync();
        }
        public async Task<int> GetCurrentMembersCount(int tripID)
        {
            return await _context.Registrations.CountAsync(u => u.TripID == tripID && u.UserStatus == "active");
        }
        public async Task<int> GetCountByUser(int userId)
        {
            return await _context.Registrations
                .CountAsync(r => r.UserID == userId && r.UserStatus == "kicked");
        }
        public async Task<int> GetLeftsCount(int tripId, int userId)
        {
            return await _context.Registrations
                .CountAsync(r => r.TripID == tripId && r.UserID == userId && r.UserStatus == "left");
        }
        public async Task<bool> AwareRepeatedRegistration(int userId, int tripId)
        {
            var exists = await _context.Registrations
            .AnyAsync(r => r.UserID == userId && r.TripID == tripId && r.UserStatus == "active");

            if (exists)
                return false;
            else return true;
        }
    }
}
