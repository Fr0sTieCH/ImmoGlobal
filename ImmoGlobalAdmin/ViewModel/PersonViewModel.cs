using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class PersonViewModel:BaseViewModel
    {
        private string searchString = "";
        private Person? selectedPerson = null;
        private bool editMode;
        private bool creationMode;
        private bool deleteDialogOpen;

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
                if(selectedPerson == null)
                {
                    selectedPerson = AllPersons.FirstOrDefault();
                }
                return selectedPerson; 
            }
            set 
            {
                if (editMode || deleteDialogOpen)
                {
                    return;
                }
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            } 
        }


        public bool EditMode => editMode;
        public bool EditModeInverted => !editMode;
        public bool DeleteDialogOpen => deleteDialogOpen;


        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }

        #region Button Commands

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
            OnPropertyChanged(nameof (EditMode));
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

        public ICommand DeletePersonButtonCommand
        {
            get
            {
                return new RelayCommand<object>(DeletePersonButtonClicked);
            }
        }

        private void DeletePersonButtonClicked(object obj)
        {
            deleteDialogOpen = true;
            OnPropertyChanged(nameof(DeleteDialogOpen));
        }

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




        #region Delete Dialog Buttons
        public ICommand DeletePersonAcceptButtonCommand
        {
            get
            {
                return new RelayCommand<object>(DeletePersonAcceptButtonClicked);
            }
        }

        private void DeletePersonAcceptButtonClicked(object obj)
        {
            selectedPerson.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this Person on {DateTime.Now}");
            DataAccessLayer.GetInstance.SaveChanges();
            deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
            OnPropertyChanged(nameof(AllPersons));
        }

        public ICommand DeletePersonCancelButtonCommand
        {
            get
            {
                return new RelayCommand<object>(DeletePersonCancelButtonClicked);
            }
        }

        private void DeletePersonCancelButtonClicked(object obj)
        {
            deleteDialogOpen = false;
            OnPropertyChanged(nameof(DeleteDialogOpen));
        }
        #endregion

        #endregion
    }
}

