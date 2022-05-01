﻿using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class PersonViewModel : BaseViewModel,IHasSearchableContent
    {

        private string searchString = "";
        private Person? selectedPerson = null;

        private bool editMode;
        private bool creationMode;

        #region Singleton
        private static PersonViewModel? instance = null;
        private static readonly object padlock = new();

        public PersonViewModel()
        {
        }

        /// <summary>
        /// returns instance of class PersonViewModel
        /// </summary>
        public static PersonViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PersonViewModel();
                    }
                    return instance;
                }
            }
        }


        #endregion


        #region BindigngProperties

        public List<Person> AllPersons
        {
            get
            {
                if (searchString == "" || searchString == null)
                {
                    return DataAccessLayer.GetInstance.GetPersonsUnfiltered();
                }
                else
                {
                    //make changes to the search logic here...
                    return DataAccessLayer.GetInstance.GetPersonsUnfiltered().Where(x => x.Name.ToLower().StartsWith(SearchString.ToLower())).ToList();
                }

            }
        }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(AllPersons));
            }
        }

        public Person? SelectedPerson
        {
            get
            {
                if (selectedPerson == null)
                {
                    selectedPerson = AllPersons.FirstOrDefault();
                }
                return selectedPerson;
            }
            set
            {
                if (editMode || DeleteDialogOpen)
                {
                    return;
                }
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }


        public bool EditMode => editMode;
        public bool EditModeInverted => !editMode;
        #endregion


        /// <summary>
        /// Method to execute the search command from the searchfield
        /// </summary>
        /// <param name="searchString"></param>
        public void SearchContent(string searchString)
        {
            //the actual search gets executed by the "AllPersons" Getter. Changes to the searchlogic have to be made there
            SearchString = searchString;
        }


        #region Button Commands

        #region Selected Person Buttons

        public ICommand EditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(EditButtonClicked);
            }
        }

        private void EditButtonClicked(object obj)
        {
            editMode = true;
            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand CancelEditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(CancelEditButtonClicked);
            }
        }

        private void CancelEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                selectedPerson = null;

            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(selectedPerson);
            }

            editMode = false;
            creationMode = false;
            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }

        public ICommand SaveEditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(SaveEditButtonClicked);
            }
        }

        private void SaveEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                Debug.WriteLine("Storing person");
                DataAccessLayer.GetInstance.StoreNewPerson(selectedPerson);
                selectedPerson = null;
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();

            }

            creationMode = false;
            editMode = false;
            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }

        #endregion

        #region DeleteDialog Override Methods
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }
        public override void DeleteAcceptButtonClicked(object obj)
        {
            selectedPerson.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this Person on {DateTime.Now}");
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(AllPersons));
            selectedPerson= null;
            OnPropertyChanged(nameof(SelectedPerson));
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);
            base.DeleteAcceptButtonClicked(obj);
        }

        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        #endregion

        public ICommand CreatePersonButtonCommand
        {
            get
            {
                return new RelayCommand<object>(CreatePersonButtonClicked);
            }
        }

        private void CreatePersonButtonClicked(object obj)
        {
            selectedPerson = new Person(true);
            OnPropertyChanged(nameof(SelectedPerson));
            editMode = true;
            creationMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        #endregion
    }
}

