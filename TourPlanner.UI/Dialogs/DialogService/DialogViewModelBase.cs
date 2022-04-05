using System.Windows;

namespace TourPlanner.UI.Dialogs.DialogService
{
    public abstract class DialogViewModelBase
    {
        public DialogResult UserDialogResult
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public IInputData InputData
        {
            get;
            private set;
        }

        public DialogViewModelBase(string message)
        {
            this.Message = message;
            // Input Data?
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result) 
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;
        }


        public void CloseDialogWithResult(Window dialog, DialogResult result, IInputData data) 
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;

            InputData = data;
        }
    }
}
