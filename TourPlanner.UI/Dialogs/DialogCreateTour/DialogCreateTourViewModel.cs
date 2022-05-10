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
        public string Tourname { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }

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

            this.Tourname = tour.Name;
            this.Description = tour.Description;
            this.From = tour.From;
            this.To = tour.To;
        }


        /*
         *  Executes when "Yes" (or "CreateTour") button is clicked
         */
        private void OnYesClicked(object parameter)
        {

            // Check if everything is set
            if (ValidateInput())
            {
                TourInput data = new TourInput {
                    Name = this.Tourname,
                    Description = this.Description,
                    From = this.From,
                    To = this.To,
            };

                // Json -> String
                string dataJson = JsonConvert.SerializeObject(data);

                this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, dataJson);
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
            if(Tourname == null || Description == null || From == null || To == null)
            {
                isValid = false;
            }

            // Do the given Locations have the correct Format?
            if(!ValidateLocation(From) || !ValidateLocation(To))
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
