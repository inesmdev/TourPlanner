﻿using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using TourPlanner.Api.Services.MapQuestService;
using TourPlanner.Models;

namespace TourPlanner.Test
{
    public class TestMapService
    {
        private IMapQuestService _mapQuestService;
        private IConfiguration config;

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder().AddUserSecrets<TestMapService>().Build();
            _mapQuestService = new MapQuestService(config);
        }

        [Test]
        public void TestGetCoordinates()
        {
            Location from = new Location() { City = "Vienna", Country = "AT", PostalCode = "1200", Street = "Handlskai 94-96" };
            Location to = new Location() { City = "Vienna", Country = "AT", PostalCode = "1190", Street = "Hardtgasse 16-30" };

            MapQuestTour result = _mapQuestService.GetTour(from, to, "abcd").Result;
            Assert.IsNotNull(result.Distance);
        }

        /*[Test]
        public void TestGetImage()
        {
            string from = "48.239178,16.387205";
            string to = "48.24622,16.374119";

            _mapQuestService.GetMap(from,to,"abcd");

            As;
        }*/
    }
}