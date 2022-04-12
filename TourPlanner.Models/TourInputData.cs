namespace TourPlanner.Models
{
    public class TourInputData
    {
        public string Tourname { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; } // Select -> Enum?
    }
}
