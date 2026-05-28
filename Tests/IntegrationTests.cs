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

            var _tripRepo = new TripRepository(_context);
            var _userRepo = new UserRepository(_context);
            var _regRepo = new RegistrationRepository(_context);
            var _picRepo = new PictureRepository(_context);

            _facade = new FacadeDB(_userRepo, _tripRepo, _picRepo, _regRepo);
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
            await _facade.RegisterTrip("Trip1", "Столби", 0, 2, 1, "Тут описание", new DateTime(2026, 1, 1), "вк.ком/ссылка");
            await _facade.RegisterTrip("Trip2", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            var sorted = await _facade.ToSort(0);
            Assert.AreEqual("Trip1", sorted[0].TripName);
        }
        [TestMethod]
        public async Task AddMember_ShouldAddMember()
        {
            await _facade.AddUser("Аня", "123321");
            await _facade.RegisterTrip("Trip", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            var users = await _context.Users.ToListAsync();
            var trip = (await _facade.GetAllTrips()).First();

            await _facade.AddMember(users[0].Id, trip.Id);
            var members = await _facade.GetMembersOfTrip(trip.Id);

            await _context.Entry(trip).ReloadAsync();

            Assert.AreEqual("Аня", members[0]);
            Assert.AreEqual(1, trip.CurMembs_amount);
        }
        [TestMethod]
        public async Task KickMember_ShouldKick()
        {
            await _facade.AddUser("Аня", "123321");
            await _context.Registrations.AddAsync(new Registration
            {
                UserID = 1,
                TripID = 1,
                UserStatus = "active"
            });
            await _context.SaveChangesAsync();
            await _facade.KickMember(1);

            var members = await _facade.GetMembersOfTrip(1);
            Assert.AreEqual(0, members.Count);
        }
        [TestMethod]
        public async Task AddMember_ShouldNotDublicateReg()
        {
            await _facade.AddUser("Аня", "123321");
            await _facade.RegisterTrip("Trip", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            var users = await _context.Users.ToListAsync();
            var trip = (await _facade.GetAllTrips()).First();

            await _facade.AddMember(users[0].Id, trip.Id);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await _facade.AddMember(users[0].Id, trip.Id); });
        }
    }
}
