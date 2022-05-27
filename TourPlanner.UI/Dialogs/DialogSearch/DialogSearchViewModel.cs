using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.Dialogs.DialogService;
using TourPlanner.Models;
using System.Collections.Generic;

namespace TourPlanner.UI.Dialogs.DialogSearch
{
    public class DialogSearchViewModel : DialogViewModelBase
    {
        // SearchCommand
        public ObservableCollection<SearchResult> SearchResults { get; private set; }

        private ICommand closeCommand = null;
        public ICommand CloseCommand
        {
            get { return closeCommand; }
            set { closeCommand = value; }
        }

        private DialogSearchViewModel() : base() {}

        public DialogSearchViewModel(string message, List<SearchResult> results)
        : base(message)
        {
            SearchResults = new ObservableCollection<SearchResult>(results);

            this.closeCommand = new RelayCommand(OnCloseClicked);
        }

        public DialogSearchViewModel(List<SearchResult> results) : base()
        {
            this.closeCommand = new RelayCommand(OnCloseClicked);

            SearchResults = new ObservableCollection<SearchResult>(results);
        } 

        private void OnCloseClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, "test");
            // Return Selected Result
        }
    }
}