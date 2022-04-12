using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;
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


        /*
         *  Executes when "Yes" (or "CreateTour") button is clicked
         */
        private void OnYesClicked(object parameter)
        {
            // Check if everything is set
            if(Tourname != null && Description != null)
            {
                TourInputData data = new TourInputData { 
                                                Tourname = this.Tourname, 
                                                Description = this.Description,
                                                From = this.From,
                                                To = this.To
                                                };

                // To Json -> String
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
    }
}
