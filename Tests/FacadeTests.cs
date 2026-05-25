using GetTripSystem.Entities;
using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{
    [TestClass]
    public sealed class FacadeTests
    {
        private Context _context;
        private FacadeDB _facade;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new Context(options);

            var _tripRepo = new TripRepository(_context);
            var _userRepo = new UserRepository(_context);
            var _regRepo = new RegistrationRepository(_context);
            var _picRepo = new PictureRepository(_context);

            _facade = new FacadeDB(_userRepo, _tripRepo, _picRepo, _regRepo);
        }
        [TestMethod]
        public async Task Add_CheckAddPic() //
        {
            string filePath = @"C:\Users\Даша\source\repos\GetTripSystem\Tests\Pic.png";
            await _facade.AddPicture(1, filePath);

            var pictures = await _facade.GetPictures(1);
            Assert.AreEqual(1, pictures.Count);
        }
        [TestMethod]
        public async Task Add_CheckAddMember()
        {
            await _facade.RegisterTrip("Поход", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await _facade.AddUser("Аня", "123321");
            await _facade.AddUser("Саня", "123321");
            await _facade.AddUser("Маша", "123321");

            await _facade.AddMember(1, 1);
            await _facade.AddMember(2, 1);

            var members = await _facade.GetMembersOfTrip(1);
            Assert.AreEqual(2, members.Count);
            Assert.AreEqual("Аня", members[0]);

        }
        [TestMethod]
        public async Task Add_CheckMembersLimit()
        {
            await _facade.RegisterTrip("Поход", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await _facade.AddUser("Аня", "123321");
            await _facade.AddUser("Саня", "123321");
            await _facade.AddUser("Маша", "123321");

            await _facade.AddMember(1, 1);
            await _facade.AddMember(2, 1);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => { await _facade.AddMember(3, 1); });

        }
        [TestMethod]
        public async Task Kick_ShouldKickMember()
        {
            await _facade.RegisterTrip("Поход", "Столби", 0, 2, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await _facade.AddUser("Аня", "123321");
            await _facade.AddMember(1, 1);
            await _facade.KickMember(1); //Убрать executeAsync?

            var members = await _facade.GetMembersOfTrip(1);
            Assert.AreEqual(null, members[0]);
        }
    }
}
