using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{
    [TestClass]
    public sealed class TripRepositoryTests
    {
        private Context _context;
        private TripRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new Context(options);
            _repo = new TripRepository(_context);
        }
        private Trip CreateTrip(string location)
        {
            return new Trip
            {
                TripName = "TripName",
                Date = DateTime.Now,
                Location = location,
                CurMembs_amount = 0,
                CreatorID = 1,
                MaxMembs_amount = 10,
                Description = "test",
                CreatorContact = "test"
            };
        }
        [TestMethod]
        public async Task Add_ShouldCreateTrip()
        {
            await _repo.Add("Поход", "Столби", 5, 5, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            var trips = _context.Trips.ToList();

            Assert.AreEqual("Поход", trips[0].TripName);
        }
        [TestMethod]
        public async Task Sort_ShouldGetSortedList()
        {
            await _context.Trips.AddRangeAsync(
                CreateTrip(location: "ААА"),
                CreateTrip(location: "ВВВ"),
                CreateTrip(location: "БББ")
            );
            await _context.SaveChangesAsync();

            var trips = await _repo.ReadAll(2);
            var sorted = _repo.SortByLocation(trips);

            Assert.AreEqual(3, sorted.Count);
            Assert.AreEqual("БББ", sorted[1].Location);
        }
        [TestMethod]
        public async Task GetCreatorsTrips_ShouldGet()
        {
            await _context.Trips.AddRangeAsync(
                CreateTrip(location: "ААА"),
                CreateTrip(location: "ВВВ"),
                CreateTrip(location: "БББ")
            );
            await _context.SaveChangesAsync();
            await _repo.Add("Поход", "Столби", 5, 5, 2, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            var trips = await _repo.GetTripsByCreatorID(1);

            Assert.AreEqual(3, trips.Count);
        }
    }
}
