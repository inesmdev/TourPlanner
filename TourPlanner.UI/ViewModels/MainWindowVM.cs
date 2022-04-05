using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Models;
using TourPlanner.UI.Dialogs;
using TourPlanner.UI.Dialogs.DialogCreateTour;

namespace TourPlanner.UI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        // Tour List
        public ObservableCollection<Tour> TourList { get; }

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);


        private ICommand openDialogCommand = null;
        public ICommand OpenDialogCommand
        {
            get { return this.openDialogCommand; }
            set { this.openDialogCommand = value; }
        }

        /*
         *  Constructor
         */
        public MainWindowVM()
        {
            // Init Tours with some data
            TourList = new ObservableCollection<Tour>(TourController.GetAllTours());
            this.openDialogCommand = new RelayCommand(OnOpenDialog);

        }


        private void OnOpenDialog(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel("Add new Tour");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out IInputData data);

            TourList.Add(new Tour() { Tourname="Crazy Tour",Description=data.ToString()});

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



        private void AddTour(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("test");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out IInputData data);

            TourList.Add(new Tour() { Tourname = data.ToString()});
        }
    }
}
