using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        private static MainViewModel? instance = null;

        /// <summary>
        /// returns instance of class MainViewModel
        /// </summary>
        public static MainViewModel? GetInstance
        {
            get
            {
                return instance;
            }
        }

        public MainViewModel(BaseViewModel viewModel)
        {
            _selectedViewModel = viewModel;
            instance = this;
        }

        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

    }
}
