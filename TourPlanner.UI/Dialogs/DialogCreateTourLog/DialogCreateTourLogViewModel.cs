using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.UI.Dialogs.DialogService;

namespace TourPlanner.UI.Dialogs.DialogCreateTourLog
{
    class DialogCreateTourLogViewModel : DialogViewModelBase
    {
        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        public EnumTourDifficulty TourDifficulty { get; set; }
        public EnumTourRating TourRating { get; set; }
        public DateTime DateTime { get; set; }
        public float TotalTime { get; set; }
        public string Comment { get; set; }

        private bool editMode = false;

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

        public DialogCreateTourLogViewModel()
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
        }

        public DialogCreateTourLogViewModel(string message) : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
        }

        public DialogCreateTourLogViewModel(string message, TourLog tourlog)
        : base(message)
        {
            Id = tourlog.Id;
            TourId = tourlog.TourId;
            TourDifficulty = tourlog.TourDifficulty;
            TourRating = tourlog.TourRating;
            DateTime = tourlog.DateTime;
            TotalTime = tourlog.TotalTime;
            Comment = tourlog.Comment;

            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            editMode = true;
        }

        /*
        *  Executes when "Yes" (or "CreateTour") button is clicked
       */
        private void OnYesClicked(object parameter)
        {
            if(editMode == false)
            {
                TourLogUserInput data = new TourLogUserInput
                {
                    DateTime = this.DateTime,
                    TourDifficulty = this.TourDifficulty,
                    TourRating = this.TourRating,
                    TotalTime = this.TotalTime,
                    Comment = this.Comment
                };
                // Json -> String
                string dataJson = JsonConvert.SerializeObject(data);

                this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, dataJson);
            }
            else
            {
                TourLog data = new TourLog
                {
                    Id = this.Id,
                    TourId = this.TourId,
                    DateTime = this.DateTime,
                    TourDifficulty = this.TourDifficulty,
                    TourRating = this.TourRating,
                    TotalTime = this.TotalTime,
                    Comment = this.Comment
                };

                // Json -> String
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
