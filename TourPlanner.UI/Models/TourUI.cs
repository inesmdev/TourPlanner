using System.Collections.ObjectModel;
using TourPlanner.Models;

namespace TourPlanner.UI.Models
{
    public class TourUI
    {
        public Tour TourData { get; set; }
        public ObservableCollection<TourLog> Tourlogs { get; set; }
    }
}
