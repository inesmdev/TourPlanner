using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
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
        public string TotalTime { get; set; }
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
            InitValues();
        }

        public DialogCreateTourLogViewModel(string message) : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
            InitValues();
        }

        public DialogCreateTourLogViewModel(string message, TourLog tourlog)
        : base(message)
        {
            

        Id = tourlog.Id;
            TourId = tourlog.TourId;
            TourDifficulty = tourlog.TourDifficulty;
            TourRating = tourlog.TourRating;
            DateTime = tourlog.DateTime;
            TotalTime = tourlog.TotalTime.ToString();
            Comment = tourlog.Comment;

            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);

            editMode = true;
        }


        void InitValues()
        {
            TourDifficulty = EnumTourDifficulty.beginner;
            TourRating = EnumTourRating.five_star;
            DateTime = DateTime.Now;
            TotalTime = "0";
            Comment = "";
        }


        /*
        *  Executes when "Yes" (or "CreateTour") button is clicked
       */
        private void OnYesClicked(object parameter)
        {
            if (Validate())
            {
                // Validate the Data 

                if (editMode == false)
                {
                    TourLogInput data = new TourLogInput
                    {
                        DateTime = this.DateTime,
                        TourDifficulty = this.TourDifficulty,
                        TourRating = this.TourRating,
                        TotalTime = Int32.Parse(this.TotalTime),
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
                        TotalTime = float.Parse(this.TotalTime),
                        Comment = this.Comment
                    };

                    // Json -> String
                    string dataJson = JsonConvert.SerializeObject(data);

                    this.CloseDialogWithResult(parameter as Window, DialogResult.Yes, dataJson);
                }
            }
        }

        /*
        *  Executes when "No" (or "Cancel") button is clicked
        */
        private void OnNoClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }


        private bool Validate()
        {
          

            // Valid Format
            // TotalTime -> Only floating point numbers allowed
            Regex regex = new Regex(@"^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!regex.IsMatch(TotalTime))
                return false;
            
            return true;
        }
    }
}
