﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TourPlanner.UI.ViewModels
{
    public abstract class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "")
        {
            ValidatePropertyName(propertyName);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void ValidatePropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("Invalid property name: " + propertyName);
            }
        }
    }
}