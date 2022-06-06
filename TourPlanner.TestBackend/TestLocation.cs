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

        [Test]
        [TestCase("Leppersdorf 23, 4612 Scharten, Austria", new string[] { "Leppersdorf 23", "4612", "Scharten", "Austria" })]
        [TestCase("Höchstädtplatz 1, 1200 Wien, Austria", new string[] { "Höchstädtplatz 1", "1200", "Wien", "Austria"})]
        public void TestParseLocation_Splitting(string locationstring, string[] locationSplit)
        {
            Location location = new Location(locationstring);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(location.Street, locationSplit[0], "Street");
                Assert.AreEqual(location.PostalCode, locationSplit[1], "Postal Code");
                Assert.AreEqual(location.City, locationSplit[2], "City");
                Assert.AreEqual(location.Country, locationSplit[3], "Country");
            });
        }
    }
}
