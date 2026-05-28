using GetTripSystem;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Moq;
using System.Windows.Documents;
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
        private Trip CreateTrip(string tripName)
        {
            return new Trip
            {
                TripName = tripName,
                Date = DateTime.Now,
                Location = "Test",
                CurMembs_amount = 0,
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
                CreateTrip(tripName: "Trip1"),
                CreateTrip(tripName: "Trip2"),
                CreateTrip(tripName: "Trip3")
            );
            await _context.SaveChangesAsync();
            var sorted = await _repo.SortByLocation();

            Assert.AreEqual(3, sorted.Count);
            Assert.AreEqual("Trip2", sorted[1].TripName);
        }
    }
}
