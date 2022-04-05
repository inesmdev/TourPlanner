using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogCreateTour
{
    class DialogCreateTourViewModel : DialogViewModelBase
    {
        private ICommand submitTourCommand = null;
        public ICommand SubmitTourCommand
        {
            get { return submitTourCommand; }
            set { submitTourCommand = value; }
        }

        private ICommand cancelCommand = null;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; }
        }

        public DialogCreateTourViewModel(string message)
        : base(message)
        {
            this.submitTourCommand = new RelayCommand(OnSubmitClicked);
            this.cancelCommand = new RelayCommand(OnCancelClicked);
        }

        private void OnSubmitClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes); //our Parameter of Data?
        }

        private void OnCancelClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }
    }
}
