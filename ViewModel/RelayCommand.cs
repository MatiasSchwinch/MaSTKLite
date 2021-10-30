using MaSTK_Lite.Interface;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MaSTK_Lite.ViewModel
{
    public sealed class RelayCommand : IRelayCommand
    {
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        private readonly Action execute;
        private readonly Func<bool>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void NotifyCanExecuteChanged()
        {
            // Este método hay que llamarlo para que actualize la vista, sin embargo con los añadidos al evento CanExecuteChanged, la vista se actualiza cuando canExecute cambia.
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object? parameter)
        {
            return canExecute?.Invoke() != false;
        }

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                execute();
            }
        }

    }

    public sealed class RelayCommand<T> : IRelayCommand<T>
    {
        private readonly Action<T?> execute;
        private readonly Predicate<T?>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T?> execute)
        {
            this.execute = execute;
        }

        public RelayCommand(Action<T?> execute, Predicate<T?> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void NotifyCanExecuteChanged()
        {
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T? parameter)
        {
            return canExecute?.Invoke(parameter) != false;
        }

        public bool CanExecute(object? parameter)
        {
            if (default(T) is not null &&
                parameter is null)
            {
                return false;
            }

            return CanExecute((T?)parameter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T? parameter)
        {
            if (CanExecute(parameter))
            {
                execute(parameter);
            }
        }

        public void Execute(object? parameter)
        {
            Execute((T?)parameter);
        }
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    }
}
