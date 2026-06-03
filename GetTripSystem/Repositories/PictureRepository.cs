using GetTripSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Repositories
{
    public class PictureRepository
    {
        private readonly Context _context;
        public PictureRepository(Context context)
        {
            _context = context;
        }
        public async Task Add(int tripId, string fileName)
        {
            var pic = new Picture
            {
                TripID = tripId,
                FileName = fileName
            };
            await _context.AddAsync(pic);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Picture>> GetAll(int tripId)
        {
            return await _context.Pictures
                .Where(r => r.TripID == tripId)
                .ToListAsync();
        }
        public async Task<Picture?> GetPictureById(int tripId)
        {
            return await _context.Pictures
                .FirstOrDefaultAsync(p => p.TripID == tripId);
        }
    }
}
