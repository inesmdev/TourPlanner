using Newtonsoft.Json;
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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int EstimatedTime { get; set; }
        public double Distance { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        //public EnumTransportType TransportType { get; set; }
        public string Summary { get; set; }

        public void GenerateSummary()
        {
            this.Summary = $"Tourname: {Name}\nFrom:{From}\nTo:{To}\nDescription: {Description}\nEstimated Time: {EstimatedTime}\nDistance: {Distance}";
        }      
    }
}