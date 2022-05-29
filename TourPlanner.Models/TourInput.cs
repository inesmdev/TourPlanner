using TourPlanner.Helper;

namespace TourPlanner.Models
{
    public class TourInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public EnumTransportType TransportType { get; set; }

        public bool Validate()
        {
            if (!Validator.isText(Name) || !Validator.isText(Description) || !Validator.isLocation(From) || !Validator.isLocation(To))
                return false;
            else
                return true;
        }
    }
}
