using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class PersonViewModel : BaseViewModel, IHasSearchableContent
    {

        private string _searchString = "";
        private Person? _selectedPerson = null;




        #region Singleton
        private static PersonViewModel? _instance = null;
        private static readonly object _padlock = new();

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
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new PersonViewModel();
                    }
                    return _instance;
                }
            }
        }


        #endregion


        #region BindigngProperties

        public override List<Person> AllPersons
        {
            get
            {
                if (_searchString == "" || _searchString == null)
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
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(AllPersons));
            }
        }



        public Person? SelectedPerson
        {
            get
            {
                if (_selectedPerson == null)
                {
                    _selectedPerson = AllPersons.FirstOrDefault();
                }
                return _selectedPerson;
            }
            set
            {
                if (_editMode || DeleteDialogOpen)
                {
                    return;
                }
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                OnPropertyChanged(nameof(TranslatedTypeOfSelectedPerson));
                OnPropertyChanged(nameof(TranslatedSexOfSelectedPerson));
            }
        }

        public string TranslatedTypeOfSelectedPerson
        {
            get
            {
                if(_selectedPerson == null) return "";
                return Application.Current.TryFindResource(_selectedPerson.SetPersonTypeString) as string ?? _selectedPerson.SetPersonTypeString;
            }
            set
            {
                if (_selectedPerson == null) return;
                string convertedString = Application.Current.TryFindResource(value) as string ?? value;
                _selectedPerson.SetPersonTypeString = convertedString;             
            }
        }

        public string TranslatedSexOfSelectedPerson
        {
            get
            {
                if (_selectedPerson == null) return "";
                return Application.Current.TryFindResource(_selectedPerson.SetSexString) as string ?? _selectedPerson.SetSexString;
            }
            set
            {
                if (_selectedPerson == null) return;
                string convertedString = Application.Current.TryFindResource(value) as string ?? value;
                _selectedPerson.SetSexString = convertedString;
            }
        }


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


        protected override void EditButtonClicked(object obj)
        {
            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));
            base.EditButtonClicked(obj);
        }
        protected override void CancelEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                _selectedPerson = null;

            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(_selectedPerson);
            }

            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));
            base.CancelEditButtonClicked(obj);
        }
        protected override void SaveEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                Debug.WriteLine("Storing person");
                DataAccessLayer.GetInstance.StoreNewPerson(_selectedPerson);
                _selectedPerson = null;
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();

            }

            base.SaveEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedPerson));
            OnPropertyChanged(nameof(AllPersons));

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
            _selectedPerson.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this Person on {DateTime.Now} with reason: ({(string)obj})");
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(AllPersons));

            base.DeleteAcceptButtonClicked(obj);

            _selectedPerson = null;
            OnPropertyChanged(nameof(SelectedPerson));
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);

        }

        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        #endregion


        #region Creaton Button
        protected override void CreateButtonClicked(object obj)
        {
            _selectedPerson = new Person(true);
            OnPropertyChanged(nameof(SelectedPerson));
            base.CreateButtonClicked(obj);
        }
        #endregion

        #endregion
    }
}

