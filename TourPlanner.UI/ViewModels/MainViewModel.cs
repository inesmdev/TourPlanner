using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.UI.ViewModels
{
    /*
     *  ViewModel for the Main Window
     */
    public class MainViewModel : BaseVM
    {
        public ObservableCollection<Tour> TourList { get; private set; }

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private RelayCommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);

        private RelayCommand editTourCommand;
        public ICommand EditTourCommand => editTourCommand ??= new RelayCommand(EditTour);

        private RelayCommand generatePdfReportCommand;
        public ICommand GeneratePdfReportCommand => generatePdfReportCommand ??= new RelayCommand(GeneratePdfReport);

        private async void GeneratePdfReport(object parameter)
        {
            if (selectedTour != null)
            {
                // Call the Api
                // Send Htttp POST Request to /Tour
                using (HttpClient client = new HttpClient())
                {
                    var jsonTour = JsonConvert.SerializeObject(selectedTour);
                    var content = new StringContent(jsonTour, Encoding.UTF8, "application/json");
                    var res = await client.PostAsync("https://localhost:5001/Report", content);

                    //res.EnsureSuccessStatusCode();
                    if (res.IsSuccessStatusCode)
                    {
                        //var httpcontent = await res.Content.ReadAsStringAsync(); //??
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Report Generated");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
  
                    }
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Report");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
            }
        }

        private RelayCommand windowLoadedCommand;
        public ICommand WindowLoadedCommand => windowLoadedCommand ??= new RelayCommand(WindowLoaded);

        /*
         *  Constructor
         */
        public MainViewModel()
        {
            TourList = new ObservableCollection<Tour>();
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
        private async void AddTour(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Create new Tour");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

            // If user clickt on "Create Tour"
            if (result == Dialogs.DialogService.DialogResult.Yes)
            {
                // Send Htttp POST Request to /Tour
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var res = await client.PostAsync("https://localhost:5001/Tour", content);

                    if (res.IsSuccessStatusCode)
                    {
                        //var httpcontent = await res.Content.ReadAsStringAsync(); //??
                        var httpcontent = await res.Content.ReadAsStringAsync(); //??


                        // String -> Tour
                        Tour tour = JsonConvert.DeserializeObject<Tour>(httpcontent);

                        // Show Tour
                        if (tour != null)
                        {
                            TourList.Add(tour);
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tour");
                            _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);

                        }
                    }
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tour");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
            }
        }


        /*
         *  Delete Tour
         */
        private async void DeleteTour(object parameter)
        {
            if (selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel($"Are your sure you want to delete Tour  \"{selectedTour.Name}\", {selectedTour.Description}? Deleted Tours cannot be recovered!"); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window);

                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(selectedTour.Id.ToString("N"), Encoding.UTF8, "application/json");
                        var res = await client.DeleteAsync($"https://localhost:5001/Tour/" + selectedTour.Id.ToString("N"));

                        res.EnsureSuccessStatusCode();
                        if (res.IsSuccessStatusCode)
                        {
                            TourList.Remove(selectedTour);
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not delete Tour");
                            _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                        }
                    }
                }
            }
        }


        /*
        *  Edit Tour
        */
        private async void EditTour(object parameter)
        {
            if (selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Edit Tour", selectedTour); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clickt on "Create Tour"
                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var res = await client.PutAsync("https://localhost:5001/Tour/" + selectedTour.Id.ToString("N"), content);

                        if (res.IsSuccessStatusCode)
                        {
                            // Update Tour in Observable Collection
                            var httpcontent = res.Content.ReadAsStringAsync().Result;

                            Tour tour = JsonConvert.DeserializeObject<Tour>(httpcontent);

                            var index = TourList.IndexOf(selectedTour);
                            TourList[index] = tour;
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not Update Tour");
                            _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                        }
                    }
                }
            }
        }


        /*
         * Load Tours when Window is loaded
         */
        private async void WindowLoaded(object parameter)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync("https://localhost:5001/Tour");

                res.EnsureSuccessStatusCode();

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    // String -> List<Tour>
                    List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(content);


                    foreach (Tour tour in tours)
                    {
                        TourList.Add(tour);
                    }
                }
                else
                {
                    // Alert Error
                }
            }
        }
    }
}
