using Microsoft.EntityFrameworkCore;
using static GetTripSystem.DAL;

namespace GetTripSystem.Repositories
{
    public class UserRepository
    {
        private readonly Context _context;
        public UserRepository (Context context)
        {
            _context = context;
        }
        public async Task Add(string name, string passwdHash)
        {
            var user = new User
            {
                Name = name,
                PasswdHash = passwdHash,
                Banned = false
            };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStatus(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            user.Banned = true;
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetUsersByIDs(List<int> userIDs)
        {
            if (userIDs == null)
                throw new Exception("Список пуст");
            else
            return await _context.Users
                .Where(u => userIDs.Contains(u.Id))
                .ToListAsync();
        }
        public async Task<User?> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        }
    }
}
