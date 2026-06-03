using GetTripSystem.Entities;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{
    [TestClass]
    public sealed class FacadeTests
    {
        private FacadeDB _facade;
        private RegistrationRepository _regRepo;
        private Context context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new Context(options);
;           var scopeFactory = new TestScopeFactory(context);
            _regRepo = new RegistrationRepository(context);

            _facade = new FacadeDB(scopeFactory);
        }
        private Registration CreateReg(int tripID)
        {
            return new Registration
            {
                UserID = 1,
                TripID = tripID,
                UserStatus = "kicked"
            };
        }
        [TestMethod]
        public async Task AddPicture_CheckAddPic()
        {
            string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectRoot = Path.GetFullPath(Path.Combine(exeDirectory, @"..\..\.."));
            string filePath = Path.Combine(projectRoot, "Pic.png");

            await _facade.AddPicture(1, filePath);
            var pictures = await _facade.GetPictures(1);
            Assert.AreEqual(1, pictures.Count);
        }
        [TestMethod]
        public async Task AddMember_CheckListMember()
        {
            await _facade.RegisterTrip("Поход", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await _facade.RegisterUser("Аня", "123321");
            await _facade.RegisterUser("Саня", "123321");

            var users = await context.Users.ToListAsync();
            var trip = (await _facade.GetAllTrips(2)).First();

            await _regRepo.Add(users[0].Id, trip.Id);
            await _regRepo.Add(users[1].Id, trip.Id);

            var members = await _facade.GetMembersOfTrip(1);
            Assert.AreEqual(2, members.Count);
        }
        [TestMethod]
        public async Task ShouldCreateUser()
        {
            await _facade.RegisterUser("Аня", "123321");
            var users = await context.Users.ToListAsync();
            Assert.AreEqual("Аня", users[0].Name);
        }
        [TestMethod]
        public async Task CheckMembersLimit()
        {
            await _facade.RegisterTrip("Поход", "Столби", 2, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await _facade.AddMember(1, 1); });

        }
        [TestMethod]
        public async Task Kick_ShouldNotGetTrip()
        {
            await _facade.RegisterTrip("Поход", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");

            await context.Registrations.AddAsync(new Registration
            {
                UserID = 1,
                TripID = 1,
                UserStatus = "kicked"
            });

            await context.SaveChangesAsync();
            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await _facade.GetTrip(1, 1); });
        }
        [TestMethod]
        public async Task BanUserCheck()
        {
            await _facade.RegisterUser("Аня", "123321");
            var users = await context.Users.ToListAsync();

            await context.Registrations.AddRangeAsync(
                CreateReg(tripID: 1),
                CreateReg(tripID: 2),
                CreateReg(tripID: 3)
            );
            await context.SaveChangesAsync();

            await _facade.CheckUserBan(1);
            Assert.AreEqual(true, users[0].Banned);
        }
    }
}
