namespace TourPlanner.Models
{
    public class TourInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public EnumTransportType TransportType { get; set; }


        // Validate TourInput ->  Call Helper Function

        public bool Validate()
        {


            return true;
        }
    }
}
