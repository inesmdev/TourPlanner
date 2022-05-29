using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.ImportExport;
using TourPlanner.UI.Models;
using TourPlanner.UI.Search;

namespace TourPlanner.UI.ViewModels
{
    /*
     *  ViewModel for the Main Window
     */
    public class MainViewModel : BaseVM
    {
        public ObservableCollection<TourUI> TourList { get; private set; }
        public ObservableCollection<TourUI> TourListBackup { get; private set; }

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

        private RelayCommand deleteTourLogCommand;
        public ICommand DeleteTourLogCommand => deleteTourLogCommand ??= new RelayCommand(DeleteTourLog);

        private RelayCommand editTourLogCommand;
        public ICommand EditTourLogCommand => editTourLogCommand ??= new RelayCommand(EditTourLog);

        private RelayCommand searchCommand;
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);

        private RelayCommand resetFilterCommand;
        public ICommand ResetFilterCommand => resetFilterCommand ??= new RelayCommand(ResetFilter);


        private RelayCommand exportTourDataCommand;
        public ICommand ExportTourDataCommand => exportTourDataCommand ??= new RelayCommand(ExportTourData);


        private RelayCommand importTourDataCommand;
        public ICommand ImportTourDataCommand => importTourDataCommand ??= new RelayCommand(ImportTourData);



        public string SearchTerm { get; set; }

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
         *  Selected TourLog
         */
        private TourLog selectedTourLog;
        public TourLog SelectedTourLog
        {
            get
            {
                return selectedTourLog;
            }
            set
            {
                if (selectedTourLog != value)
                {
                    selectedTourLog = value;
                    RaisePropertyChangedEvent("SelectedTourLog");
                }
            }
        }



        /*
         *  Constructor
         */
        public MainViewModel()
        {
            // Initalize TourList
            TourList = new ObservableCollection<TourUI>();
            TourListBackup = null;

        }


        private void ResetFilter(object parameter)
        {
            LoadListFromBackup();
        }

        private void LoadListFromBackup()
        {
            if (TourListBackup != null)
            {
                TourList = TourListBackup;
                TourListBackup = null;
                RaisePropertyChangedEvent("TourList");
            }
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
         *  Search
         */
        private void Search(object parameter)
        {
            if (SearchTerm != null)
            {
                List<TourUI> result = SearchService.Search(TourList, SearchTerm);
                // Selected Tour = clicked on
                // Selcted tourlog
                TourListBackup = TourList;
                TourList = new ObservableCollection<TourUI>(result);
                RaisePropertyChangedEvent("TourList");
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
                        var httpcontent = await res.Content.ReadAsStringAsync(); // -> Json Content?

                        // String -> Tour
                        Tour tour = JsonConvert.DeserializeObject<Tour>(httpcontent);
                        
                        LoadListFromBackup();

                        // Show Tour
                        if (tour != null)
                        {
                            TourList.Add(new TourUI()
                            {
                                TourData = tour,
                                Tourlogs = new ObservableCollection<TourLog>() //Tourlogs do not have any logs when created
                            });
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tour :/");
                            _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);

                        }
                    }
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create Tour :/");
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
                        var res = await client.DeleteAsync($"https://localhost:5001/Tour/" + selectedTour.TourData.Id.ToString("N"));

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
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Edit Tour", selectedTour.TourData); // Add Tour Details
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clicked on "Create Tour"
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

                            TourList[TourList.IndexOf(selectedTour)].TourData = tour;
                        }
                        else
                        {
                            Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not update Tour :/");
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
                        TourUI tourui = new TourUI()
                        {
                            TourData = tour
                        };
                        

                        // Fetch tourlogs for each tour
                        var tourlogRes = await client.GetAsync("https://localhost:5001/all/" + tour.Id.ToString("N"));
                        

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

        private async void DeleteTourLog(object parameter)
        {
            if (selectedTourLog != null)
            {
                // Send Htttp POST Request to /Tour
                using (HttpClient client = new HttpClient())
                {
                    var res = await client.DeleteAsync("https://localhost:5001/TourLog/" + selectedTourLog.Id.ToString("N"));

                    if (res.IsSuccessStatusCode)
                    {
                        TourList[TourList.IndexOf(selectedTour)].Tourlogs.Remove(selectedTourLog); // 
                    }
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create TourLog");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
            }

        }

        private async void EditTourLog(object parameter)
        {
            if(selectedTourLog != null)
            {
                // Pop Up Window
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTourLog.DialogCreateTourLogViewModel("Edit TourLog", selectedTourLog); 
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);


                // Send Htttp POST Request to /Tour
                using (HttpClient client = new HttpClient())
                {
                    // Tourid, Tourdata
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var res = await client.PutAsync("https://localhost:5001/TourLog/" + selectedTourLog.Id.ToString("N"), content);

                    if (res.IsSuccessStatusCode)
                    {
                        // Update TourLog in Tourlogs list
                        var databe = await res.Content.ReadAsStringAsync();
                        var newlog = JsonConvert.DeserializeObject<TourLog>(databe);

                        TourList[TourList.IndexOf(selectedTour)].Tourlogs[TourList[TourList.IndexOf(selectedTour)].Tourlogs.IndexOf(selectedTourLog)] = newlog;
                    }
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not edit Tourlog");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
            }
        }


        private void ImportTourData(object parameter)
        {
            // FileInput
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON file (*.json)|*.json | Text file (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                var fileContent = File.ReadAllText(openFileDialog.FileName);

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                       // var content = new StringContent(data, Encoding.UTF8, "application/json");
                       // var res = await client.PostAsync("https://localhost:5001/TourLog/" + selectedTourLog.Id.ToString("N"), content);


                    }
                }
                catch
                {
                    // Error Popup
                }

                List<TourUI> tours = JsonConvert.DeserializeObject<List<TourUI>>(fileContent);

                RaisePropertyChangedEvent("TourList");

                // If tours dont already exist -> change if data has changed, else create bew tizr

            }
        }


        private void ExportTourData(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON file (*.json)|*.json | Text file (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(TourList));
        }
    }
}
