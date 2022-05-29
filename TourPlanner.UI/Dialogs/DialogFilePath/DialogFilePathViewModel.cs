using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogFilePath
{
    public class DialogFilePathViewModel : DialogViewModelBase
    {
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

        public DialogFilePathViewModel() : base() { }

        public DialogFilePathViewModel(string message)
        : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
        }


        /*
         *  Executes when "Yes" (or "CreateTour") button is clicked
         */
        private void OnYesClicked(object parameter)
        {

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