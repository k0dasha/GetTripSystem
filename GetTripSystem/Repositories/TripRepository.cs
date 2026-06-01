using GetTripSystem.Entities;
using Microsoft.EntityFrameworkCore;
using static GetTripSystem.DAL;

namespace GetTripSystem.Repositories
{
    public class TripRepository
    {
        private readonly Context _context;

        public TripRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<Trip>> ReadAll(int userId)
        {
            return await _context.Trips.AsNoTracking().Where(u => u.CurMembs_amount != u.MaxMembs_amount && u.CreatorID != userId).ToListAsync();
        }
        public async Task<List<Trip>> GetTripsByCreatorID(int creatorId)
        {
            return await _context.Trips.AsNoTracking().Where(u => u.CreatorID == creatorId).ToListAsync();
        }
        public async Task<Trip?> ReadByID(int tripId)
        {
            return await _context.Trips.FirstOrDefaultAsync(u => u.Id == tripId);
        }
        public async Task<List<Trip>> GetTripsByIDs(List<int> tripIDs)
        {
            if (tripIDs == null)
                throw new Exception("Список пуст");
            else
                return await _context.Trips.AsNoTracking()
                    .Where(u => tripIDs.Contains(u.Id))
                    .ToListAsync();
        }
        public List<Trip> SortByDate(List<Trip> list)
        {
            return list.OrderBy(c => c.Date).ToList();
        }
        public List<Trip> SortByLocation(List<Trip> list)
        {
            return list.OrderByDescending(c => c.Location).ToList();
        }
        public async Task Add(string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact)
        {
            var trip = new Trip
            {
                TripName = tripName,
                Location = location,
                CurMembs_amount = curMembs,
                MaxMembs_amount = maxMembs,
                CreatorID = creatorID,
                Description = desc,
                Date = date,
                CreatorContact = creatorContact
            };
            await _context.AddAsync(trip);
            await _context.SaveChangesAsync();
        }
        public async Task Update(int id, int maxMembs, string desc, DateTime date, string creatorContact)
        {
            await _context.Trips
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.MaxMembs_amount, maxMembs)
                .SetProperty(x => x.Description, desc)
                .SetProperty(x => x.Date, date)
                .SetProperty(x => x.CreatorContact, creatorContact));
        }
        public async Task IncreaseMembersCount(int id)
        {
            await _context.Trips
            .Where(t => t.Id == id && t.CurMembs_amount < t.MaxMembs_amount)
            .ExecuteUpdateAsync(s => s
            .SetProperty(t => t.CurMembs_amount, t => t.CurMembs_amount + 1));

        }
        public async Task DecreaseMembersCount(int id)
        {
            await _context.Trips
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(t => t.CurMembs_amount, t => t.CurMembs_amount - 1));

        }
        public Task<int> GetMaxMembersCount(int tripID)
        {
            return _context.Trips
                .Where(r => r.Id == tripID)
                .Select(r => r.MaxMembs_amount)
                .FirstOrDefaultAsync();
        }
    }
}
