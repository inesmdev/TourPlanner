using System;

namespace TourPlanner.Models
{
    public enum EnumTourRating
    {
        one_star,
        two_star,
        three_star,
        four_star,
        five_star
    }

    public enum EnumTourDifficulty
    {
        beginner,
        intermediate,
        expert
    }

    public class TourLog
    {
        public Guid Id { get; set; }
        public Guid TourId { get; set;}
        public DateTime DateTime { get; set; }
        public EnumTourRating TourRating { get; set; }
        public EnumTourDifficulty TourDifficulty { get; set; }
        public float TotalTime { get; set; }
        public string Comment { get; set; }
    }
}
