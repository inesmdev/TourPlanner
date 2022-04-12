using System.Windows;

namespace TourPlanner.UI.Dialogs.DialogService
{
    public static class DialogService
    {
        /*
         *  Open a Dialog Window which returns no data
         */     
        public static DialogResult OpenDialog(DialogViewModelBase vm, Window owner)
        {
            DialogWindow win = new DialogWindow();

            if (owner != null)
                win.Owner = owner;
            win.DataContext = vm;
            win.ShowDialog();

            DialogResult result = (win.DataContext as DialogViewModelBase).UserDialogResult;

            return result;
        }


        /*
         *  Open a Dialog Window which contains some input fields
         */
        public static DialogResult OpenDialog(DialogViewModelBase vm, Window owner, out string data)
        {
            DialogWindow win = new DialogWindow();
            if (owner != null)
                win.Owner = owner;

            win.DataContext = vm;
            win.ShowDialog();

            DialogResult result = (win.DataContext as DialogViewModelBase).UserDialogResult; 

            _ = (result == DialogResult.No) ? data = null : data = (win.DataContext as DialogViewModelBase).Data;

            return result;
        }
    }
}
