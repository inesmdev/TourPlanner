using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogCreateTour
{
    public class DialogCreateTourViewModel : DialogViewModelBase
    {
        public string Tourname { get; set; }
        public string Description { get; set; }

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

        public DialogCreateTourViewModel(string message)
        : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            this.Tourname = null;
            this.Description = null;
        }

        private void OnYesClicked(object parameter)
        {
            // Check if everything is set
            if(Tourname != null && Description != null)
            {
                IInputData data = new TourInputData { Tourname = this.Tourname, Description = this.Description};

                this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, data);
            }

            // Pop Up -> Ok Window

        }

        private void OnNoClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }
    }
}
