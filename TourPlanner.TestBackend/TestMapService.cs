using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.IO;
using TourPlanner.Api.Services.MapQuestService;
using TourPlanner.Api.Services.TourService;
using TourPlanner.Models;
using Moq;

namespace TourPlanner.Test
{
    public class TestMapService
    {
        private IMapQuestService _mapQuestService;
        private ITourService _tourService;
        private IConfiguration config;

        [SetUp]
        public void Setup()
        {
            var repo = new Mock<DAL.Repositories.ITourRepository>();
            var logger = new Mock<ILogger<TourService>>();
            config = new ConfigurationBuilder().AddUserSecrets<TestMapService>().Build();
            _mapQuestService = new MapQuestService(config);
            _tourService = new TourService(repo.Object, _mapQuestService, logger.Object);
        }

        [Test]
        [TestCase(0, new string[] { "Vienna", "AT", "1200", "Handlskai 94-96" }, new string[] { "Vienna" , "AT", "1190", "Hardtgasse 16"})]
        [TestCase(1, new string[] { "Vienna", "AT", "1200", "Höchstädtplatz 1" }, new string[] { "Vienna", "AT", "1200", "Höchstädtplatz 10" })]
        public void TestGetCoordinates_ValidAddress(int useless, string[] fromstring, string[] tostring)
        {
            Location from = new Location() { 
                City = fromstring[0], 
                Country = fromstring[1], 
                PostalCode = fromstring[2], 
                Street = fromstring[3] 
            };
            Location to = new Location() {
                City = tostring[0],
                Country = tostring[1],
                PostalCode = tostring[2],
                Street = tostring[3] 
            };

            MapQuestTour result = _mapQuestService.GetTour(from, to, "abcd").Result;
            Assert.IsNotNull(result.Distance);
        }

        [Test]
        [TestCase("Höchstädtplatz 1", new string[] { "Vienna", "AT", "1200", "Handlskai 94-96" })]
        [TestCase("1200", new string[] { "Vienna", "", "", "Handlskai 94-96" })]
        public void TestGetCoordinates_InvalidAddress(string fromstring, string[] tostring)
        {
            Location from = new Location() { Street = fromstring };
            Location to = new Location() { City = tostring[0], Country = tostring[1], PostalCode = tostring[2], Street = tostring[3] };

            MapQuestTour result = _mapQuestService.GetTour(from, to, "abcd").Result;

            Assert.AreEqual(result.Distance, 0.0);
        }

        [Test]
        public void TestAddTour_ValidInputTour()
        {
            TourInput input = new TourInput() {
                From = "Höchstädtplatz 1, 1200 Wien, Austria",
                Description = "Test Description",
                To = "Höchstädtplatz 2, 1200 Wien, Austria",
                Name = "Test",
                TransportType = EnumTransportType.fastest
            };

            Tour res = _tourService.Add(input);

            Assert.IsNotNull(res);
        }

        [Test]
        public void TestAddTour_IvnalidInputTour()
        {
            TourInput input = new TourInput()
            {
                From = "Höchstädtplatz 1",
                Description = "Test Description",
                To = "Höchstädtplatz 2",
                Name = "Test",
                TransportType = EnumTransportType.fastest
            };

            Tour res = _tourService.Add(input);

            Assert.IsNull(res);
        }

        /*[Test]
        public void TestGetImage()
        {
            string from = "48.239178,16.387205";
            string to = "48.24622,16.374119";
            string basepath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.ToString();
            string path = basepath + "\\TourPlanner.Api\\StaticFiles\\abcd.jpg";
            bool existCheck = File.Exists(path);

            _mapQuestService.GetMap(from,to,"abcd");

            Assert.IsTrue(existCheck);
        }*/
    }
}