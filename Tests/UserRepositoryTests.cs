using GetTripSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{

    [TestClass]
    public sealed class UserRepositoryTests
    {
        private Context _context;
        private UserRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase("TestDb_User")
                .Options;

            _context = new Context(options);
            _repo = new UserRepository(_context);
        }
        [TestMethod]
        public async Task Add_ShouldCreateUser()
        {
            await _repo.Add("Анна", "хэш337");

            var users = _context.Users.ToList();
            Assert.AreEqual(false, users[0].Banned);
        }
        [TestMethod]
        public async Task Update_GetChangedStatus()
        {
            await _repo.Add("Анна", "хэш337");
            var users = _context.Users.ToList();
            var user = users[0];

            await _repo.UpdateStatus(user.Id);
            Assert.AreEqual(true, user.Banned);

        }
    }
        
}
