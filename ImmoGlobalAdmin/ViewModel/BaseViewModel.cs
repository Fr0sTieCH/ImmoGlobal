using Notifications.Wpf.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.Model;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.View;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        private bool deleteDialogOpen;
        public bool DeleteDialogOpen => deleteDialogOpen;
        public bool DeleteDialogNotOpen => !deleteDialogOpen;

        internal void ShowNotification(string titel, string message, NotificationType type)
        {
            var notificationManager = new NotificationManager();
            notificationManager.ShowAsync(new NotificationContent { Title = titel, Message = message, Type = type },
                    areaName: "WindowArea", expirationTime: new TimeSpan(0, 0, 2));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region ButtonCommands

        #region DeleteButtonCommand
        public ICommand DeleteButtonCommand => new RelayCommand<object>(DeleteButtonClicked);
        public virtual void DeleteButtonClicked(object obj)
        {
            //sets the bool to open the "Are You sure?" Dialog
            deleteDialogOpen = true;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));

        }
        #endregion



        #region DeleteDialogButtonCommands
        public ICommand DeleteAcceptButtonCommand => new RelayCommand<object>(DeleteAcceptButtonClicked);
        public virtual void DeleteAcceptButtonClicked(object obj)
        {
            deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));
        }

        public ICommand DeleteCancelButtonCommand => new RelayCommand<object>(DeleteCancelButtonClicked);
        public virtual void DeleteCancelButtonClicked(object obj) 
        {
            deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));
        }
        #endregion


        #endregion
    }
}
