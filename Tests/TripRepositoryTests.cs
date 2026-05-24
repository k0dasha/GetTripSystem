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
                .UseInMemoryDatabase("TestDb_Trip")
                .Options;

            _context = new Context(options);
            _repo = new TripRepository(_context);
        }

        [TestMethod]
        public async Task Add_ShouldCreateTrip()
        {
            await _repo.Add("Поход", "Столби", 5, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            var trips = _context.Trips.ToList();

            Assert.AreEqual(1, trips.Count);
            Assert.AreEqual("Поход", trips[0].TripName);
        }
    }
}
