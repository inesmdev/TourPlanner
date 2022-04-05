using System.Windows;

namespace TourPlanner.UI.Dialogs.DialogService
{
    public static class DialogService
    {
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

        public static DialogResult OpenDialog(DialogViewModelBase vm, Window owner, out IInputData data)
        {
            DialogWindow win = new DialogWindow();
            if (owner != null)
                win.Owner = owner;

            win.DataContext = vm;
            win.ShowDialog();
            DialogResult result = (win.DataContext as DialogViewModelBase).UserDialogResult; //??

            if(result == DialogResult.No)
            {
                data = null;
            }
            else
            {
                data = (win.DataContext as DialogViewModelBase).InputData;
            }

            return result;
        }

    }
}
