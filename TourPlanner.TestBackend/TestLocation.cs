using NUnit.Framework;
using TourPlanner.Exceptions;
using TourPlanner.Models;

namespace TourPlanner.TestBackend
{
    class TestLocation
    {
        [SetUp]
        public void Setup()
        { }

        [Test]
        public void TestParseLocation_ValidLocation()
        {
            string validLocation = "Leppersdorf 23, 4612 Scharten, Austria";

            Assert.DoesNotThrow(
                delegate
                {
                    Location loc = new Location(validLocation);
                });
        }


        [Test]
        public void TestParseLocation_InvalidLocation()
        {
            string invalidLocation = "Leppersdorf 234612 SchartenAustria";

            Assert.Throws(
               Is.TypeOf<CouldNotParseException>().And.Message.EqualTo("Failed to parse location"),
               delegate
               {
                   Location loc = new Location(invalidLocation);
               });
        }
    }
}
