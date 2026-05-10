using GetTripSystem;
using GetTripSystem.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Moq;
using System.Windows.Documents;

namespace GetTripSystem.Tests
{
    [TestClass]
    public sealed class TripTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            TripRepository _tripRepository = new TripRepository();
            DateTime date1 = new DateTime(2026, 5, 9);
            _tripRepository.Add(1, "GoHOME", "Sweet home", 0, 10, 1, "", date1, "");
            
            Assert.IsNotNull(_tripRepository.ReadAll());
        }
    }
}
