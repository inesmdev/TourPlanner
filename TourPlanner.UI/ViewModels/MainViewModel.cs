using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.Models;

namespace TourPlanner.UI.ViewModels
{
    /*
     *  ViewModel for the Main Window
     */
    public class MainViewModel : BaseVM
    {
        public ObservableCollection<TourUI> TourList { get; private set; }

        private RelayCommand addTourCommand;
        public ICommand AddTourCommand => addTourCommand ??= new RelayCommand(AddTour);

        private RelayCommand deleteTourCommand;
        public ICommand DeleteTourCommand => deleteTourCommand ??= new RelayCommand(DeleteTour);

        private RelayCommand editTourCommand;
        public ICommand EditTourCommand => editTourCommand ??= new RelayCommand(EditTour);

        private RelayCommand generatePdfReportCommand;
        public ICommand GeneratePdfReportCommand => generatePdfReportCommand ??= new RelayCommand(GeneratePdfReport);

        private RelayCommand windowLoadedCommand;
        public ICommand WindowLoadedCommand => windowLoadedCommand ??= new RelayCommand(WindowLoaded);

        private RelayCommand addTourLogCommand;
        public ICommand AddTourLogCommand => addTourLogCommand ??= new RelayCommand(AddTourLog);

        /*
         *  Selected Tour
         */
        private TourUI selectedTour;
        public TourUI SelectedTour
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
         *  Constructor
         */
        public MainViewModel()
        {
            TourList = new ObservableCollection<TourUI>();
        }


        /*
         *  Generate Pdf Report
         */
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
                            TourList.Add(new TourUI()
                            {
                                TourData = tour,
                                Tourlogs = new ObservableCollection<TourLog>() // no tour logs at start
                            });
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
         *  Delete Tour -> Delete Tour and Tourlogs per Tour
         */
        private async void DeleteTour(object parameter)
        {
            if (selectedTour != null)
            {
                // (1. Delete Tourlogs)

                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel($"Are your sure you want to delete Tour  \"{selectedTour.TourData.Name}\", {selectedTour.TourData.Description}? Deleted Tours cannot be recovered!"); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window);

                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(selectedTour.TourData.Id.ToString("N"), Encoding.UTF8, "application/json");
                        var res = await client.DeleteAsync($"https://localhost:5001/Tour/" + selectedTour.TourData.Id.ToString("N"));

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

                // 2. Delete Tour


            }
        }


        /*
        *  Edit Tour
        */
        private async void EditTour(object parameter)
        {
            if (selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Edit Tour", selectedTour.TourData); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clickt on "Create Tour"
                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var res = await client.PutAsync("https://localhost:5001/Tour/" + selectedTour.TourData.Id.ToString("N"), content);

                        if (res.IsSuccessStatusCode)
                        {
                            // Update Tour in Observable Collection
                            var httpcontent = res.Content.ReadAsStringAsync().Result;

                            Tour tour = JsonConvert.DeserializeObject<Tour>(httpcontent);

                            var index = TourList.IndexOf(selectedTour);
                            TourList[index].TourData = tour;
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
                // Get all Tours
                var res = await client.GetAsync("https://localhost:5001/Tour");

                res.EnsureSuccessStatusCode();

                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    // String -> List<Tour>
                    List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(content);

                    foreach (Tour tour in tours)
                    {
                        TourUI tourui = new TourUI();
                        tourui.TourData = tour;

                        // Get Tourlog per Tour
                        var tourlogRes = await client.GetAsync("https://localhost:5001/TourLog/all/" + tour.Id.ToString("N"));
                        
                        if (tourlogRes.IsSuccessStatusCode)
                        {
                            var tourlogContent = await tourlogRes.Content.ReadAsStringAsync();
                            List<TourLog> tourlogs = JsonConvert.DeserializeObject<List<TourLog>>(tourlogContent);
                            tourui.Tourlogs = new ObservableCollection<TourLog>(tourlogs);
                        }
                        else
                        {
                            // empty list -> no logs
                            tourui.Tourlogs = new ObservableCollection<TourLog>();
                        }

                        TourList.Add(tourui);
                    }
                }
                else
                {
                    // Alert Error

                }
            }
        }


        /*
         *  Create new TourLog
         */
        private async void AddTourLog(object parameter)
        {
            if(selectedTour != null)
            {
               TourLog tourlog;

                // Pop up Window
               Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTourLog.DialogCreateTourLogViewModel("Create new Tourlog");
               Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clickt on "Create Tour"
               if (result == Dialogs.DialogService.DialogResult.Yes)
               {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        // Deserialze data
                        TourLogUserInput dataJson = JsonConvert.DeserializeObject<TourLogUserInput>(data);
                        // Serialize data
                        dataJson.TourId = selectedTour.TourData.Id;
                        
                        string dataJson2 = JsonConvert.SerializeObject(dataJson);


                        var content = new StringContent(dataJson2, Encoding.UTF8, "application/json");
                        var res = await client.PostAsync("https://localhost:5001/TourLog", content);

                        if (res.IsSuccessStatusCode)
                        {
                            //var httpcontent = await res.Content.ReadAsStringAsync(); //??
                            var httpcontent = await res.Content.ReadAsStringAsync(); //??

                            // String -> Tour
                            tourlog = JsonConvert.DeserializeObject<TourLog>(httpcontent);

                            // Show Tour
                            if (tourlog != null)
                            {
                                // add tourlog
                                // selected tour
                                var index = TourList.IndexOf(selectedTour);
                                TourList[index].Tourlogs.Add(tourlog);
                            }
                            else
                            {
                                Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tourlog");
                                _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                            }
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tourlog");
                            _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                        }
                    }
               }
            }
        }
    }
}
