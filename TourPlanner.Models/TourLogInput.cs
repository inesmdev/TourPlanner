using System;
using TourPlanner.Helper;

namespace TourPlanner.Models
{
    public class TourLogInput
    {
        public Guid TourId { get; set; }
        public DateTime DateTime { get; set; }
        public EnumTourRating TourRating { get; set; }
        public EnumTourDifficulty TourDifficulty { get; set; }
        public float TotalTime { get; set; }
        public string Comment { get; set; }


        public bool Validate()
        {
            if (!Validator.isText(Comment))
                return false;
            else
                return true;
        }
    }
}
