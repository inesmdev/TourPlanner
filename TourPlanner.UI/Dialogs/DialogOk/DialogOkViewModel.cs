using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogOk 
{ 

    public class DialogOkViewModel : DialogService.DialogViewModelBase
    {
        private ICommand okCommand = null;
        public ICommand OkCommand
        {
            get { return okCommand; }
            set { okCommand = value; }
        }

        public DialogOkViewModel(string message)
        : base(message)
        {
            this.okCommand = new RelayCommand(OnOkClicked);
        }

        private void OnOkClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
        }
    }
}
