using System;

namespace TourPlanner.Models
{
    public enum EnumTransportType
    {
        fastest,
        shortest,
        pedestrian,
        bicycle
    }

    public class Tour
    {
        public Guid TourId { get; set; }
        public string Tourname { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int EstimatedTime { get; set; }
        public double Distance { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
        public EnumTransportType TransportType { get; set; }
    }
}