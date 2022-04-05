﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.Dialogs.DialogService;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.UI.Dialogs.DialogYesNo
{
    class DialogYesNoViewModel : DialogViewModelBase
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

        public DialogYesNoViewModel(string message)
        : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
        }

        private void OnYesClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
        }

        private void OnNoClicked(object parameter)
        {
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }
    }
}
