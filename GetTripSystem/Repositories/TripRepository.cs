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
        public TripRepository()
        {
        }

        public async Task<List<Trip>> ReadAll()
        {
            return await _context.Trips.ToListAsync();
        }
        public async Task<List<Trip>> SortByDate()
        {
            return await _context.Trips.OrderBy(c => c.Date).ToListAsync();
        }
        public async Task<List<Trip>> SortByLocation()
        {
            return await _context.Trips.OrderBy(c => c.Location).ToListAsync();
        }
        public async Task Add(int id, string tripName, string location, int curMembs, int maxMembs,
        int creatorID, string desc, DateTime date, string creatorContact)
        {
            var trip = new Trip
            {
                Id = id,
                TripName = tripName,
                Location = location,
                CurMembs_amount = 0,
                MaxMembs_amount = maxMembs,
                CreatorID = creatorID,
                Description = desc,
                Date = date,
                CreatorContact = creatorContact
            };
            await _context.AddAsync(trip);
            await _context.SaveChangesAsync();
        }
        public async void Delete(int id)
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

            await _context.SaveChangesAsync();
        }
    }
}
