using MaSTK_Lite.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaSTK_Lite.ViewModel
{
    #region AsyncRelayCommand
    public sealed class AsyncRelayCommand : ObservableObject, IAsyncRelayCommand
    {
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        internal static readonly PropertyChangedEventArgs CanBeCanceledChangedEventArgs = new(nameof(CanBeCanceled));

        internal static readonly PropertyChangedEventArgs IsCancellationRequestedChangedEventArgs = new(nameof(IsCancellationRequested));

        internal static readonly PropertyChangedEventArgs IsRunningChangedEventArgs = new(nameof(IsRunning));

        private readonly Func<Task>? execute;
        private readonly Func<CancellationToken, Task>? cancelableExecute;
        private readonly Func<bool>? canExecute;

        private CancellationTokenSource? cancellationTokenSource;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<Task> execute)
        {
            this.execute = execute;
        }

        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute)
        {
            this.cancelableExecute = cancelableExecute;
        }

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
        {
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
        }

        private TaskNotifier? executionTask;

        public Task? ExecutionTask
        {
            get => this.executionTask;
            private set
            {
                if (SetPropertyAndNotifyOnCompletion(ref this.executionTask, value, _ =>
                {
                    OnPropertyChanged(IsRunningChangedEventArgs);
                    OnPropertyChanged(CanBeCanceledChangedEventArgs);
                }))
                {
                    OnPropertyChanged(IsRunningChangedEventArgs);
                    OnPropertyChanged(CanBeCanceledChangedEventArgs);
                }
            }
        }

        public bool CanBeCanceled => this.cancelableExecute is not null && IsRunning;

        public bool IsCancellationRequested => this.cancellationTokenSource?.IsCancellationRequested == true;

        public bool IsRunning => ExecutionTask?.IsCompleted == false;

        public void NotifyCanExecuteChanged()
        {
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object? parameter)
        {
            return this.canExecute?.Invoke() != false;
        }

        public void Execute(object? parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        public Task ExecuteAsync(object? parameter)
        {
            if (CanExecute(parameter))
            {
                if (this.execute is not null)
                {
                    return ExecutionTask = this.execute();
                }

                this.cancellationTokenSource?.Cancel();

                CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource = new();

                OnPropertyChanged(IsCancellationRequestedChangedEventArgs);

                return ExecutionTask = this.cancelableExecute!(cancellationTokenSource.Token);
            }

            return Task.CompletedTask;
        }

        public void Cancel()
        {
            this.cancellationTokenSource?.Cancel();

            OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            OnPropertyChanged(CanBeCanceledChangedEventArgs);
        }
    }
#endregion

    #region AsyncRelayCommand<T>
    public sealed class AsyncRelayCommand<T> : ObservableObject, IAsyncRelayCommand<T>
    {
        private readonly Func<T?, Task>? execute;
        private readonly Func<T?, CancellationToken, Task>? cancelableExecute;
        private readonly Predicate<T?>? canExecute;

        private CancellationTokenSource? cancellationTokenSource;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<T?, Task> execute)
        {
            this.execute = execute;
        }

        public AsyncRelayCommand(Func<T?, CancellationToken, Task> cancelableExecute)
        {
            this.cancelableExecute = cancelableExecute;
        }

        public AsyncRelayCommand(Func<T?, Task> execute, Predicate<T?> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public AsyncRelayCommand(Func<T?, CancellationToken, Task> cancelableExecute, Predicate<T?> canExecute)
        {
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
        }

        private TaskNotifier? executionTask;

        public Task? ExecutionTask
        {
            get => this.executionTask;
            private set
            {
                if (SetPropertyAndNotifyOnCompletion(ref this.executionTask, value, _ =>
                {
                    // When the task completes
                    OnPropertyChanged(AsyncRelayCommand.IsRunningChangedEventArgs);
                    OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
                }))
                {
                    // When setting the task
                    OnPropertyChanged(AsyncRelayCommand.IsRunningChangedEventArgs);
                    OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
                }
            }
        }

        public bool CanBeCanceled => this.cancelableExecute is not null && IsRunning;

        public bool IsCancellationRequested => this.cancellationTokenSource?.IsCancellationRequested == true;

        public bool IsRunning => ExecutionTask?.IsCompleted == false;

        public void NotifyCanExecuteChanged()
        {
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T? parameter)
        {
            return this.canExecute?.Invoke(parameter) != false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            _ = ExecuteAsync(parameter);
        }

        public void Execute(object? parameter)
        {
            _ = ExecuteAsync((T?)parameter);
        }

        public Task ExecuteAsync(T? parameter)
        {
            if (CanExecute(parameter))
            {
                if (this.execute is not null)
                {
                    return ExecutionTask = this.execute(parameter);
                }

                this.cancellationTokenSource?.Cancel();

                CancellationTokenSource cancellationTokenSource = this.cancellationTokenSource = new();

                OnPropertyChanged(AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);

                return ExecutionTask = this.cancelableExecute!(parameter, cancellationTokenSource.Token);
            }

            return Task.CompletedTask;
        }

        public Task ExecuteAsync(object? parameter)
        {
            return ExecuteAsync((T?)parameter);
        }

        public void Cancel()
        {
            this.cancellationTokenSource?.Cancel();

            OnPropertyChanged(AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
            OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
        }
    }
    #endregion

    #region ObservableObject
    public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event PropertyChangingEventHandler? PropertyChanging;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
        }

        protected bool SetProperty<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
        {
            if (comparer.Equals(field, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<T>(T oldValue, T newValue, Action<T> callback, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<T>(T oldValue, T newValue, IEqualityComparer<T> comparer, Action<T> callback, [CallerMemberName] string? propertyName = null)
        {
            if (comparer.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(model, newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<TModel, T>(T oldValue, T newValue, IEqualityComparer<T> comparer, TModel model, Action<TModel, T> callback, [CallerMemberName] string? propertyName = null)
            where TModel : class
        {
            if (comparer.Equals(oldValue, newValue))
            {
                return false;
            }

            OnPropertyChanging(propertyName);

            callback(model, newValue);

            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetPropertyAndNotifyOnCompletion([NotNull] ref TaskNotifier? taskNotifier, Task? newValue, [CallerMemberName] string? propertyName = null)
        {
            return SetPropertyAndNotifyOnCompletion(taskNotifier ??= new(), newValue, static _ => { }, propertyName);
        }

        protected bool SetPropertyAndNotifyOnCompletion([NotNull] ref TaskNotifier? taskNotifier, Task? newValue, Action<Task?> callback, [CallerMemberName] string? propertyName = null)
        {
            return SetPropertyAndNotifyOnCompletion(taskNotifier ??= new(), newValue, callback, propertyName);
        }

        protected bool SetPropertyAndNotifyOnCompletion<T>([NotNull] ref TaskNotifier<T>? taskNotifier, Task<T>? newValue, [CallerMemberName] string? propertyName = null)
        {
            return SetPropertyAndNotifyOnCompletion(taskNotifier ??= new(), newValue, static _ => { }, propertyName);
        }

        protected bool SetPropertyAndNotifyOnCompletion<T>([NotNull] ref TaskNotifier<T>? taskNotifier, Task<T>? newValue, Action<Task<T>?> callback, [CallerMemberName] string? propertyName = null)
        {
            return SetPropertyAndNotifyOnCompletion(taskNotifier ??= new(), newValue, callback, propertyName);
        }

        private bool SetPropertyAndNotifyOnCompletion<TTask>(ITaskNotifier<TTask> taskNotifier, TTask? newValue, Action<TTask?> callback, [CallerMemberName] string? propertyName = null)
            where TTask : Task
        {
            if (ReferenceEquals(taskNotifier.Task, newValue))
            {
                return false;
            }

            bool isAlreadyCompletedOrNull = newValue?.IsCompleted ?? true;

            OnPropertyChanging(propertyName);

            taskNotifier.Task = newValue;

            OnPropertyChanged(propertyName);

            if (isAlreadyCompletedOrNull)
            {
                callback(newValue);

                return true;
            }

            async void MonitorTask()
            {
                try
                {
                    await newValue!;
                }
                catch
                {
                }

                if (ReferenceEquals(taskNotifier.Task, newValue))
                {
                    OnPropertyChanged(propertyName);
                }

                callback(newValue);
            }

            MonitorTask();

            return true;
        }

        private interface ITaskNotifier<TTask>
            where TTask : Task
        {
            TTask? Task { get; set; }
        }

        protected sealed class TaskNotifier : ITaskNotifier<Task>
        {
            internal TaskNotifier()
            {
            }

            private Task? task;

            Task? ITaskNotifier<Task>.Task
            {
                get => this.task;
                set => this.task = value;
            }

            public static implicit operator Task?(TaskNotifier? notifier)
            {
                return notifier?.task;
            }
        }

        protected sealed class TaskNotifier<T> : ITaskNotifier<Task<T>>
        {
            internal TaskNotifier()
            {
            }

            private Task<T>? task;

            Task<T>? ITaskNotifier<Task<T>>.Task
            {
                get => this.task;
                set => this.task = value;
            }

            public static implicit operator Task<T>?(TaskNotifier<T>? notifier)
            {
                return notifier?.task;
            }
        }
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
    }
    #endregion
}