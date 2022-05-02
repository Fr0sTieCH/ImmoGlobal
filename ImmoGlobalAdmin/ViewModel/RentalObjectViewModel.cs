﻿using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class RentalObjectViewModel:BaseViewModel
    {
        private RentalObject rentalObjectToDisplay;

        #region Singleton
        private static RentalObjectViewModel? instance = null;
        private static readonly object padlock = new();

        public RentalObjectViewModel()
        {
        }

        /// <summary>
        /// returns instance of class RentalObjectViewModel
        /// </summary>
        public static RentalObjectViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RentalObjectViewModel();
                    }
                    return instance;
                }
            }
        }




        #endregion

        #region Binding Properties
        public RentalObject RentalObjectToDisplay 
        {
            get 
            { 
                return rentalObjectToDisplay; 
            }
            set
            {
                rentalObjectToDisplay = value; 
                OnPropertyChanged(nameof(RentalObjectToDisplay));
            }
        }




        #endregion

        #region Delete Dialog Buttons
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }
        public override void DeleteAcceptButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);
            base.DeleteAcceptButtonClicked(obj);
        }
        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }

        protected override void CancelEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                
            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(rentalObjectToDisplay);
            }

            base.CancelEditButtonClicked(obj);
            OnPropertyChanged(nameof(RentalObjectToDisplay));
            OnPropertyChanged(nameof(AllRealEstates));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewRentalObject(RentalObjectToDisplay);
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();
            }

            base.SaveEditButtonClicked(obj);
            OnPropertyChanged(nameof(AllRealEstates));
            OnPropertyChanged(nameof(RentalObjectToDisplay));

        }
        #endregion
    }
}
