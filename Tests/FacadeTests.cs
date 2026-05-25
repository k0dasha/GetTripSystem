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
            await _facade.RegisterTrip("Поход", "Столби", 0, 5, 1, "Тут описание", DateTime.Now, "вк.ком/ссылка");
            await _facade.AddMember(1, 1);
            await _facade.AddMember(2, 1);

            var members = await _facade.GetMembersOfTrip(1);
            Assert.AreEqual(2, members.Count);


        }
    }
}
