using System.ComponentModel;
using System.Threading.Tasks;

namespace MaSTK_Lite.Interface
{
    public interface IAsyncRelayCommand : IRelayCommand, INotifyPropertyChanged
    {
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        Task? ExecutionTask { get; }

        bool CanBeCanceled { get; }

        bool IsCancellationRequested { get; }

        bool IsRunning { get; }

        Task ExecuteAsync(object? parameter);

        void Cancel();

    }
    public interface IAsyncRelayCommand<in T> : IAsyncRelayCommand, IRelayCommand<T>
    {
        Task ExecuteAsync(T? parameter);
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    }

}
