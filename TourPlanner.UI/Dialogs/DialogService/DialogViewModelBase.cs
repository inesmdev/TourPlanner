using System.Windows;

namespace TourPlanner.UI.Dialogs.DialogService
{
    public abstract class DialogViewModelBase
    {
        /*
         *  Button Clicked
         */
        public DialogResult UserDialogResult
        {
            get;
            private set;
        }

        /*
         * Message, which should be displayed in the Yes-No-Dialog Box
         */
        public string Message
        {
            get;
            private set;
        }

        /*
         *  Data
         */
        public string Data
        {
            get;
            private set;
        }


        /*
         *  Constructor
         */
        public DialogViewModelBase()
        {
            this.Message = null;
            this.Data = null;
        }

        public DialogViewModelBase(string message)
        {
            this.Message = message;
            this.Data = null;
        }


        public void CloseDialogWithResult(Window dialog, DialogResult result) 
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result, string data) 
        {
            this.UserDialogResult = result;
            if (dialog != null)
                dialog.DialogResult = true;

            Data = data;
        }
    }
}
