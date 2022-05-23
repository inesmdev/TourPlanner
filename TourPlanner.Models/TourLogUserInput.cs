using System;

namespace TourPlanner.Models
{
    public class TourLogUserInput
    {
        public Guid TourId { get; set; }
        public DateTime DateTime { get; set; }
        public EnumTourRating TourRating { get; set; }
        public EnumTourDifficulty TourDifficulty { get; set; }
        public float TotalTime { get; set; }
        public string Comment { get; set; }
    }
}
