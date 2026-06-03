using GetTripSystem.Entities;
using GetTripSystem.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{
    namespace GetTripSystem.Tests
    {
        [TestClass]
        public sealed class IntegrationTests
        {
            private Context _context;
            private SqliteConnection _connection;
            private FacadeDB _facade;

            [TestInitialize]
            public void Setup()
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlite(_connection)
                    .Options;

                _context = new Context(options);
                _context.Database.EnsureCreated();

                var scopeFactory = new TestScopeFactory(_context);

                _facade = new FacadeDB(scopeFactory);
            }
            [TestCleanup]
            public void Cleanup()
            {
                _context.Dispose();
                _connection.Close();
            }
            [TestMethod]
            public async Task ToSortByDate_CheckCorrectTripList()
            {
                await _facade.RegisterUser("Аня", "123321");
                await _facade.RegisterTrip("Trip1", "Столби", 0, 2, 1, "Тут описание", new DateTime(2026, 1, 1), "вк.ком/ссылка");
                await _facade.RegisterTrip("Trip2", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

                var trips = _context.Trips.ToList();

                var sorted = _facade.ToSort(0, trips);
                Assert.AreEqual("Trip1", sorted[0].TripName);
            }

            [TestMethod]
            public async Task KickMember_ShouldKick()
            {
                await _facade.RegisterUser("Аня", "123321");
                await _facade.RegisterTrip("Trip", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
                await _context.Registrations.AddAsync(new Registration
                {
                    UserID = 1,
                    TripID = 1,
                    UserStatus = "active"
                });
                await _context.SaveChangesAsync();
                await _facade.KickMember(1,1);

                var members = await _facade.GetMembersOfTrip(1);
                Assert.AreEqual(0, members.Count);
            }
            [TestMethod]
            public async Task AddMember_ShouldNotDublicateReg()
            {
                await _facade.RegisterUser("Аня", "123321");
                await _facade.RegisterTrip("Trip", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
                var users = await _context.Users.ToListAsync();
                var trip = (await _facade.GetAllTrips(2)).First();

                await _facade.AddMember(users[0].Id, trip.Id);
                await Assert.ThrowsAsync<ArgumentException>(async () => { await _facade.AddMember(users[0].Id, trip.Id); });
            }
        }
    }
}
