namespace TourPlanner.Models
{
    public class Location
    {
        // vgl. MapQuest Location ?
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public Location(string location)
        {
            ParseLocation(location);
        }

        private void ParseLocation(string location)
        {
            // Street, City, Country, Postal Code
            // Marchfeldstraße 25/9, 1200 Wien, Austria

            char[] delimiterChars = { ' ', ',' };

            string[] words = location.Split(delimiterChars);

            this.Street = words[0] + " " + words[1];
            this.PostalCode = words[2];
            this.City = words[3];
            this.County = words[4];
        }
    }
}
