using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogCreateTour
{
    public class DialogCreateTourViewModel : DialogViewModelBase
    {
        public Guid Id { get; set; }
        public string Tourname { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public EnumTransportType TransportType { get; set; }
        public DateTime CreationDate { get; set; }
        public int EstimatedTime { get; set; }
        public double Distance { get; set; }
        public string Summary { get; set; }

        private bool editMode = false;

        private ICommand yesCommand = null;
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        public DialogCreateTourViewModel() : base() { }

        public DialogCreateTourViewModel(string message)
        : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            this.Tourname = null;
            this.Description = null;
        }

        public DialogCreateTourViewModel(string message, Tour tour)
        : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            // Set everything
            this.Id = tour.Id;
            this.Tourname = tour.Name;
            this.Description = tour.Description;
            this.From = tour.From;
            this.To = tour.To;
            this.TransportType = tour.TransportType;
            this.CreationDate = tour.CreationDate;
            this.EstimatedTime = tour.EstimatedTime;
            this.Distance = tour.Distance;

            editMode = true;
        }


        /*
         *  Executes when "Yes" (or "CreateTour") button is clicked
         */
        private void OnYesClicked(object parameter)
        {
            if (ValidateInput())
            {
                if (editMode == true)
                {
                    Tour data = new Tour
                    {
                        Id = this.Id,
                        Name = this.Tourname,
                        Description = this.Description,
                        From = this.From,
                        To = this.To,
                        TransportType = this.TransportType,
                        CreationDate = this.CreationDate,
                        EstimatedTime = this.EstimatedTime,
                        Distance = this.Distance,
                    };

                    // Json -> String
                    string dataJson = JsonConvert.SerializeObject(data);

                    this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, dataJson);
                }
                else
                {
                    TourInput data = new TourInput
                    {
                        Name = this.Tourname,
                        Description = this.Description,
                        From = this.From,
                        To = this.To,
                        TransportType = this.TransportType
                    };

                    // Json -> String
                    string dataJson = JsonConvert.SerializeObject(data);

                    this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, dataJson);
                }
            }
        }

        /*
        *  Executes when "No" (or "Cancel") button is clicked
        */
        private void OnNoClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }


        private bool ValidateLocation(string location)
        {
            string locationPattern = @"[A-Za-z]\w+ [0-9]{1,3}(\/[0-9]{1,3})*, ([0-9]{4}) [A-Za-z]\w+, [A-Za-z]\w+";
            Regex regex = new Regex(locationPattern, RegexOptions.IgnoreCase);

            var match = regex.Match(location);

            if (match.Success)
                return true;
            else
                return false;
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Are all Inputs filled out?
            if (Tourname == null || Description == null || From == null || To == null)
            {
                isValid = false;
            }

            // Do the given Locations have the correct Format?
            if (!ValidateLocation(From) || !ValidateLocation(To))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}