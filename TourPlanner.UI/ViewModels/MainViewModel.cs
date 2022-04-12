using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL.TourService;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    /*
     *  ViewModel for the Main Window
     */
    public class MainViewModel : BaseVM
    {
        ITourService _tourService;
        
        public ObservableCollection<Tour> TourList { get; }

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);


        /*
         *  Constructor
         */
        public MainViewModel(ITourService tourService)
        {
            this._tourService = tourService;
            
            // Init Tours with some data
            //TourList = new ObservableCollection<Tour>(TourController.GetAllTours());
        }

        // Selected Tour
        private Tour selectedTour;
        public Tour SelectedTour
        {
            get
            {
                return selectedTour;
            }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    RaisePropertyChangedEvent("SelectedTour");
                }
            }
        }


        /*
         *  Create new Tour
         */
        private void AddTour(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("asdf");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

           /*
            if(result == Dialogs.DialogService.DialogResult.Yes)
            {
                // Add Tour to DB
                //Tour tour = TourController.AddTour(JsonConvert.DeserializeObject<Models.TourInputData>(data)); //???

                // Show Tour
                if(tour != null)
                {
                    TourList.Add(tour);
                }
            } */ 
         
        }
    }
}
