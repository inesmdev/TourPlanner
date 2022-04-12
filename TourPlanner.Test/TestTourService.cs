using NUnit.Framework;
using TourPlanner.BL.TourService;
using TourPlanner.Test.Mocks;

namespace TourPlanner.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            ITourService tourService = new TourService(new MockTourRepository());
        }

        [Test]
        public void TestAddTour()
        {
            Assert.Pass();
        }
    }
}