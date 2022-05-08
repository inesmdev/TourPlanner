using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Api.Services;
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
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);
           
            // If user clickt on "Create Tour"
            if(result == Dialogs.DialogService.DialogResult.Yes)
            {    
                // Send Htttp POST Request to /Tour
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var res = await client.PostAsync("https://localhost:44314/Tour", content);

                    //res.EnsureSuccessStatusCode();
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
            if(selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel($"Delete Tour\nId:{selectedTour.Id}, Tourname: {selectedTour.Name}"); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window);

                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(selectedTour.Id.ToString("N"), Encoding.UTF8, "application/json");
                        var res = await client.DeleteAsync($"https://localhost:44314/Tour/"+selectedTour.Id.ToString("N"));

                        //res.EnsureSuccessStatusCode();
                        if (res.IsSuccessStatusCode)
                        {
                            // Remove tour
                            // Tour Deleted
                            // Event auslösen dass Daten neu lädt aus DB? -> schönste Lösung
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
        private void EditTour(object parameter)
        {
            throw new System.NotImplementedException();
        }


        /*
         * Load Tours when Window is loaded
         */
        private async void WindowLoaded(object parameter)
        {
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync("https://localhost:44314/Tour");

                res.EnsureSuccessStatusCode();

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    // String -> List<Tour>
                    List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(content);

                   
                    foreach(Tour tour in tours)
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
