using TourPlanner.Exceptions;
using TourPlanner.Helper;

namespace TourPlanner.Models
{
    public class Location
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public Location()
        {}

        public Location(string location)
        {
            ParseLocation(location);
        }

        // Regex
        private void ParseLocation(string location)
        {
            if (!Validator.isLocation(location))
                throw new CouldNotParseException("Failed to parse location");


            char[] delimiterChars = { ' ', ',' };

            string[] words = location.Split(delimiterChars);

            this.Street = words[0] + " " + words[1];
            this.PostalCode = words[3];
            this.City = words[4];
            this.Country = words[6];
            /*this.Street = words[0];
            this.PostalCode = words[1];
            this.City = words[2];
            this.Country = words[3];*/
        }
    }
}
