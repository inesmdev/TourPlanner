using NUnit.Framework;
using TourPlanner.Api.Services.MapQuestService;
using TourPlanner.Models;
using TourPlanner.Test.Mocks;

namespace TourPlanner.Test
{
    public class TestMapService
    {
        private IMapQuestService _mapQuestService;

        [SetUp]
        public void Setup()
        {
            _mapQuestService = new MapQuestService();
        }

        [Test]
        public void TestGetCoordinates()
        {
            Location from = new Location() { City = "Vienna", Country = "AT", PostalCode = "1200", Street = "Handlskai 94-96" };
            Location to = new Location() { City = "", Country = "AT", PostalCode = "", Street = "Prater" };

            MapQuestTour result = _mapQuestService.GetTour(from, to).Result;
            Assert.IsNotNull(result.Distance);
        }
    }
}