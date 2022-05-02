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
using ImmoGlobalAdmin.Helpers;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        protected bool deleteDialogOpen = false;
        public bool DeleteDialogOpen => deleteDialogOpen;
        public bool DeleteDialogNotOpen => !deleteDialogOpen;


        protected bool editMode = false;
        public bool EditMode => editMode;
        public bool EditModeInverted => !editMode;


        public virtual List<Person> AllPersons => DataAccessLayer.GetInstance.GetPersonsUnfiltered();
        public virtual List<BankAccount> AllBankAccounts => DataAccessLayer.GetInstance.GetBankAccountsUnfiltered();
        public virtual List<RealEstate> AllRealEstates => DataAccessLayer.GetInstance.GetRealEstatesUnfiltered();

        protected bool creationMode;

        public string[] ObjectTypeIcons
        {
            get
            { 
                //gets the descriptions of the enumvalues of RentalObjectType and returns them as an string[] (Excluding RentalObjectType.RealEstateBaseObject)
               return ((int[])Enum.GetValues(typeof(RentalObjectType))).Where(x => x!=0).Select(x => EnumTools.GetDescription((RentalObjectType)x)).ToArray();
            }
        }

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


        #region Edit/Create ButtonCommands

        public ICommand EditButtonCommand =>new RelayCommand<object>(EditButtonClicked);
        protected virtual void EditButtonClicked(object obj)
        {
            editMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand SaveEditButtonCommand=>  new RelayCommand<object>(SaveEditButtonClicked);
        protected virtual void SaveEditButtonClicked(object obj)
        {
           
            creationMode = false;
            editMode = false;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }

        public ICommand CancelEditButtonCommand => new RelayCommand<object>(CancelEditButtonClicked);
        protected virtual void CancelEditButtonClicked(object obj)
        {
            editMode = false;
            creationMode = false;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand CreateButtonCommand=> new RelayCommand<object>(CreateButtonClicked);
        protected virtual void CreateButtonClicked(object obj)
        {
            editMode = true;
            creationMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }


        #endregion

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
