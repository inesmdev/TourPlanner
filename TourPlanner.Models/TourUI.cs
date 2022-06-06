using System.Collections.ObjectModel;
using System.ComponentModel;
using TourPlanner.Models;

namespace TourPlanner.UI.Models
{
    public class TourUI :INotifyPropertyChanged
    {
        private Tour tourData;
        public Tour TourData {
            get
            {
                return tourData;
            }
            set
            {
                tourData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TourData"));
            }
        }

        private string imagePath;
        public string ImagePath {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImagePath"));

            }
        }

        public ObservableCollection<TourLog> Tourlogs { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
