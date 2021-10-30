using System.Windows.Input;

namespace MaSTK_Lite.Interface
{
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        ///     Notifica que la propiedad ICommand.CanExecute ha cambiado.
        /// </summary>
        void NotifyCanExecuteChanged();
    }

    public interface IRelayCommand<in T> : IRelayCommand
    {
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        bool CanExecute(T? parameter);

        void Execute(T? parameter);
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    }
}
