using ImmoGlobalAdmin.Commands;
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
        private bool deleteDialogOpen;
        
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
        #endregion
    }
}
