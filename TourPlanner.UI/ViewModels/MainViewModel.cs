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
using TourPlanner.UI.Models;
using TourPlanner.UI.Search;

namespace TourPlanner.UI.ViewModels
{
    /*
     *  ViewModel for the main window
     */
    public class MainViewModel : BaseVM
    {
        private const string DB_ERROR = "Database connection failed. Please try again later";

        public ObservableCollection<TourUI> TourList { get; private set; }
        public ObservableCollection<TourUI> TourListBackup { get; private set; }
        public string SearchTerm { get; set; }
        
        /*
        *  Selected tour
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
         *  Selected tourlog
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
         *  Commands
         */
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

        private RelayCommand smallFontCommand;
        public ICommand SmallFontCommand => smallFontCommand ??= new RelayCommand(SmallFont);

        private RelayCommand bigFontCommand;
        public ICommand BigFontCommand => bigFontCommand ??= new RelayCommand(BigFont);

        private RelayCommand mediumFontCommand;
        public ICommand MediumFontCommand => mediumFontCommand ??= new RelayCommand(MediumFont);

        private RelayCommand hugeFontCommand;
        public ICommand HugeFontCommand => hugeFontCommand ??= new RelayCommand(HugeFont);


        /*
         *  Constructor
         */
        public MainViewModel()
        {
            TourList = new ObservableCollection<TourUI>();
            TourListBackup = null;
        }
     
        /*
         *  Generate pdf report
         *  TODO: File download
         */
        private async void GeneratePdfReport(object parameter)
        {
            if (selectedTour != null)
            {
                // Call the Api
                // Send Htttp POST Request to /Tour
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var jsonTour = JsonConvert.SerializeObject(selectedTour.TourData);
                        var content = new StringContent(jsonTour, Encoding.UTF8, "application/json");
                        var res = await client.PostAsync("https://localhost:5001/Report/test.pdf", content);

                        if (res.IsSuccessStatusCode)
                        {
                            // var httpcontent = await res.Content.ReadAsStringAsync(); //??
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
                catch
                {
                    MessageBox.Show(DB_ERROR);
                }
            }
        }


        /*
         *  FontSize
         */
        private void SmallFont(object paramter)
        {
            Properties.Settings.Default.FontSize="Small";
            Properties.Settings.Default.Save();
        }
   
        private void MediumFont(object paramter)
        {
            Properties.Settings.Default.FontSize = "Medium";
            Properties.Settings.Default.Save();
        }
        
        private void BigFont(object paramter)
        {
            Properties.Settings.Default.FontSize = "Big";
            Properties.Settings.Default.Save();
        }

        private void HugeFont(object paramter)
        {
            Properties.Settings.Default.FontSize = "Huge";
            Properties.Settings.Default.Save();
        }


        /*
         *  Im memory search
         */
        private void Search(object parameter)
        {
            if (SearchTerm != null)
            {
                List<TourUI> result = SearchService.Search(TourList, SearchTerm);
                TourListBackup = TourList;
                TourList = new ObservableCollection<TourUI>(result);
                RaisePropertyChangedEvent("TourList");
            }
        }


        /*
        *  Reset filtered tourlist (search)
        */
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
         *  Tour
         */
        // Add
        private async void AddTour(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Create new Tour");
            Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

            // If user clicked on "Create Tour"
            if (result == Dialogs.DialogService.DialogResult.Yes)
            {
                // Send Htttp POST Request to /Tour
                try {
                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var res = await client.PostAsync("https://localhost:5001/Tour", content);

                        if (res.IsSuccessStatusCode)
                        {
                            var httpcontent = await res.Content.ReadAsStringAsync(); // TODO: (improvement) -> Json Content?

                            // String -> Tour
                            Tour tour = JsonConvert.DeserializeObject<Tour>(httpcontent);

                            // Make sure that all tours are displayed (undo filter if there is any)
                            LoadListFromBackup();

                            // Show Tour
                            if (tour != null)
                            {
                                TourList.Add(new TourUI()
                                {
                                    TourData = tour,
                                    Tourlogs = new ObservableCollection<TourLog>(), //Tourlogs do not have any logs when created
                                    ImagePath = $"https://localhost:5001/StaticFiles/{tour.Id.ToString()}.jpg"
                                });
                            }
                            else
                            {
                                Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create tour. Please try again.");
                                _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);

                            }
                        }
                  
                    else
                    {
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not create tour. Please try again.");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
                }
                catch
                {
                    MessageBox.Show(DB_ERROR);
                }
            }
        }

        // Delete tour -> Delete tour and all tourlogs for this tour (db setting cascade)
        private async void DeleteTour(object parameter)
        {
            if (selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel($"Are your sure you want to delete Tour  \"{selectedTour.TourData.Name}\", {selectedTour.TourData.Description}? Deleted Tours cannot be recovered. All TourLogs for this tour will be deleted.");
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window);

                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    try
                    {

                        using (HttpClient client = new HttpClient())
                        {
                            var res = await client.DeleteAsync($"https://localhost:5001/Tour/" + selectedTour.TourData.Id.ToString("N"));

                            if (res.IsSuccessStatusCode)
                            {
                                TourList.Remove(selectedTour);
                            }
                            else
                            {
                                Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not delete tour. Please try again.");
                                _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show(DB_ERROR);
                    }
                }
            }
        }

        // Edit tour
        private async void EditTour(object parameter)
        {
            if (selectedTour != null)
            {
                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTour.DialogCreateTourViewModel("Edit tour", selectedTour.TourData); 
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clicked on "Create Tour"
                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Http POST Request to /Tour
                    try
                    {
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
                                // RaisePropertyChangedEvent("TourList");

                            }
                            else
                            {
                                Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Could not update Tour :/");
                                _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show(DB_ERROR);
                    }
                }
            }
        }

        // Load tours when window is loaded   
        private async void WindowLoaded(object parameter)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Get all Tours
                    var res = await client.GetAsync("https://localhost:5001/Tour");

                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();

                        // String -> List<Tour>
                        List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(content);

                        foreach (Tour tour in tours)
                        {
                            TourUI tourui = new TourUI()
                            {
                                TourData = tour,
                                ImagePath = $"https://localhost:5001/StaticFiles/{tour.Id.ToString()}.jpg"
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
                        Dialogs.DialogService.DialogViewModelBase popup = new Dialogs.DialogOk.DialogOkViewModel("Error loading tours. Please restart the application.");
                        _ = Dialogs.DialogService.DialogService.OpenDialog(popup, parameter as Window);
                    }
                }
            }
            catch
            {
                MessageBox.Show(DB_ERROR);
            }
        }

        /*
         *  Tourlogs
         */
        // Create new tourlog
        private async void AddTourLog(object parameter)
        {
            if (selectedTour != null)
            {
                TourLog tourlog;

                Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogCreateTourLog.DialogCreateTourLogViewModel("Create new Tourlog");
                Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, parameter as Window, out string data);

                // If user clicked on "Create Tour"
                if (result == Dialogs.DialogService.DialogResult.Yes)
                {
                    // Send Htttp POST Request to /Tour
                    using (HttpClient client = new HttpClient())
                    {
                        // Deserialze data
                        TourLogInput dataJson = JsonConvert.DeserializeObject<TourLogInput>(data);
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

        // Delete tourlog
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

        // Edit tourlog
        private async void EditTourLog(object parameter)
        {
            if (selectedTourLog != null)
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


        /*
         *  Import tourdata from .json or .txt file
         */
        private async void ImportTourData(object parameter)
        {
            // FileInput
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);

                // Valid File Content? -> Json?
                try
                {
                    List<TourUI> tours = JsonConvert.DeserializeObject<List<TourUI>>(fileContent);

                    using (HttpClient client = new HttpClient())
                    {
                        var content = new StringContent(fileContent, Encoding.UTF8, "application/json");
                        _ = await client.PutAsync("https://localhost:5001/Import", content);

                        LoadListFromBackup();
                        TourList = new ObservableCollection<TourUI>(tours);
                        RaisePropertyChangedEvent("TourList");
                    }
                }
                catch
                {
                    // Error Popup
                    MessageBox.Show("Error importing tours");
                }       
            }
        }

        /*
         *  Export tourdata
         */
        private void ExportTourData(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(TourList));
        }
    }
}
