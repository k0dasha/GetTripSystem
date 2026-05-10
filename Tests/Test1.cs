using GetTripSystem;
using GetTripSystem.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Moq;
using System.Windows.Documents;

namespace Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TripRepository tripR = new TripRepository();
            DateTime date1 = new DateTime(2026, 5, 9);
            tripR.Add(1, "GoHOME", "Sweet home", 0, 10, 1, "", date1, "");
            
            Assert.IsNotNull(tripR.ReadAll());
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var mockRepo = new Mock<UserRepository>();
            DateTime date1 = new DateTime(2026, 5, 9);
            mockRepo.Setup(x => x.Add(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = 2;
                u.Name = "Sandy";
                u.PasswdHash = "123abc";
                u.Banned = false;
                return u;
            });
        }
    }
}
