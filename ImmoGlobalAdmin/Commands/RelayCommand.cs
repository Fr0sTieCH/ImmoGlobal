using System;
using System.Diagnostics;
using System.Windows.Input;

namespace ImmoGlobalAdmin.Commands
{
    internal class RelayCommand<T> : ICommand
    {

        #region Members
        private readonly Action<T>? _Execute = null;
        private readonly Predicate<T>? _CanExecute = null;
        #endregion


        #region Constructors
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public RelayCommand(Action<T> Execute) : this(Execute, CanExecute: null) { }

        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="Execute">Execution logic</param>
        /// <param name="CanExecute">Execution status logic</param>
        public RelayCommand(Action<T>? Execute, Predicate<T>? CanExecute)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        #endregion


        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object Parameter)
        {
            return _CanExecute == null || _CanExecute((T)Parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object Parameter)
        {
            _Execute?.Invoke((T)Parameter);
        }
        #endregion
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    }
}
