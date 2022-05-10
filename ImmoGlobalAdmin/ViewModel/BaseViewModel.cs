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
using System.Collections.ObjectModel;

namespace ImmoGlobalAdmin.ViewModel
{
    internal abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        protected bool _deleteDialogOpen = false;
        public bool DeleteDialogOpen => _deleteDialogOpen;
        public bool DeleteDialogNotOpen => !_deleteDialogOpen;


        protected bool _editMode = false;
        public bool EditMode => _editMode;
        public bool EditModeInverted => !_editMode;
        public bool CreationMode => _creationMode;
        public bool CreationModeInverted => !_creationMode;


        public virtual List<Person> AllPersons => DataAccessLayer.GetInstance.GetPersonsUnfiltered();
        public virtual List<Person> AllEmployees => DataAccessLayer.GetInstance.GetEmployeesUnfiltered();
        public virtual List<Person> AllPrivatePersons => DataAccessLayer.GetInstance.GetPrivatePersonsUnfiltered();
        public virtual List<Person> AllCompanies => DataAccessLayer.GetInstance.GetCompaniesUnfiltered();
        public virtual List<BankAccount> AllBankAccounts => DataAccessLayer.GetInstance.GetBankAccountsUnfiltered();
        public virtual List<RealEstate> AllRealEstates => DataAccessLayer.GetInstance.GetRealEstatesUnfiltered();
        public virtual List<Transaction> AllTransactions => DataAccessLayer.GetInstance.GetTransactionsUnfiltered();

        protected bool _creationMode;

        public string[] ObjectTypeIcons
        {
            get
            {
                //gets the descriptions of the enumvalues of RentalObjectType and returns them as an string[] (Excluding RentalObjectType.RealEstateBaseObject)
                return ((int[])Enum.GetValues(typeof(RentalObjectType))).Where(x => x != 0).Select(x => EnumTools.GetDescription((RentalObjectType)x)).ToArray();
            }
        }

        //gets the enumnames of TransactionType
        public string[] TransactionTypes => Enum.GetNames(typeof(TransactionType));
        //Translated Enumnames of TransactionType
        public string[] TransactionTypesTranslated => TransactionTypes.Select(x => Application.Current.TryFindResource(x) as string ?? x).ToArray();

        //gets the enumnames of Sex
        public string[] SexArray => Enum.GetNames(typeof(Sex));
        //Translated EnumNames of Sex
        public string[] SexArrayTranslated => SexArray.Select(x => Application.Current.TryFindResource(x) as string ?? x).ToArray();

        //gets the enumnames of PersonType
        public string[] PersonTypes => Enum.GetNames(typeof(PersonType));
        //Translated Enumnames of PersonType
        public string[] PersonTypesTranslated => PersonTypes.Select(x => Application.Current.TryFindResource(x) as string ?? x).ToArray();



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

        public ICommand EditButtonCommand => new RelayCommand<object>(EditButtonClicked);
        protected virtual void EditButtonClicked(object obj)
        {
            _editMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand SaveEditButtonCommand => new RelayCommand<object>(SaveEditButtonClicked);
        protected virtual void SaveEditButtonClicked(object obj)
        {

            _creationMode = false;
            _editMode = false;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }

        public ICommand CancelEditButtonCommand => new RelayCommand<object>(CancelEditButtonClicked);
        protected virtual void CancelEditButtonClicked(object obj)
        {
            _editMode = false;
            _creationMode = false;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand CreateButtonCommand => new RelayCommand<object>(CreateButtonClicked);
        protected virtual void CreateButtonClicked(object obj)
        {
            _editMode = true;
            _creationMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }


        #endregion

        #region DeleteButtonCommand
        public ICommand DeleteButtonCommand => new RelayCommand<object>(DeleteButtonClicked);
        public virtual void DeleteButtonClicked(object obj)
        {
            //sets the bool to open the "Are You sure?" Dialog
            _deleteDialogOpen = true;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));

        }
        #endregion

        #region DeleteDialogButtonCommands
        public ICommand DeleteAcceptButtonCommand => new RelayCommand<object>(DeleteAcceptButtonClicked);
        public virtual void DeleteAcceptButtonClicked(object obj)
        {
            _deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));
        }

        public ICommand DeleteCancelButtonCommand => new RelayCommand<object>(DeleteCancelButtonClicked);
        public virtual void DeleteCancelButtonClicked(object obj)
        {
            _deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(DeleteDialogNotOpen));
        }
        #endregion

        public void DisposeDB()
        {
            DataAccessLayer.GetInstance.DisposeContext();
        }
        #endregion
    }
}
