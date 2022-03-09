using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.UI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        // Tour List
        public ObservableCollection<Tour> TourList { get; }

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

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
                if(selectedTour != value)
                {
                    selectedTour = value;
                    RaisePropertyChangedEvent("SelectedTour");
                }
            }
        }

        public MainWindowVM()
        {
            // Init Tours with some data
            TourList = new ObservableCollection<Tour>();
            TourList.Add(new Tour() { Tourname = "TestTour1", Description = "Die beste Tour!" });
            TourList.Add(new Tour() { Tourname = "TestTour2", Description="Bergauf & Bergab"});
        }

        private void AddTour(object commandParameter)
        {
            TourList.Add(new Tour() { Tourname="Crazy Tour", Description="Supercoole Tour"});
        }
    }
}
