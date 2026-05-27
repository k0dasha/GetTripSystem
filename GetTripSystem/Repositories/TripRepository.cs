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

        public async Task<List<Trip>> ReadAll()
        {
            return await _context.Trips.ToListAsync();
        }
        public async Task<Trip?> ReadByID(int tripId)
        {
            return await _context.Trips.FindAsync(tripId);
        }
        public async Task<List<Trip>> SortByDate()
        {
            return await _context.Trips.OrderBy(c => c.Date).ToListAsync();
        }
        public async Task<List<Trip>> SortByLocation()
        {
            return await _context.Trips.OrderBy(c => c.Location).ToListAsync();
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
        public async Task Delete(int id) //0
        {
            await _context.Trips
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
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
        public async Task<List<Trip>> GetCreatorsTrips(int id) //0
        {
            return await _context.Trips.Where(c => c.CreatorID == id).ToListAsync();
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
