using System;
using System.Windows.Input;

namespace MessageRoutine.Sample.Wpf.Models
{
    public class MessageAsyncCommand<T> : ICommand
    {
#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public Action<T>? Action { get; }
        public string Message { get; }

        public MessageAsyncCommand(string message) : this(message, null) { }
        public MessageAsyncCommand(string message, Action<T>? action)
        {
            Message = message;
            Action = action;
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
            => Action?.Invoke(await Program.Manager.StartRoutineAsync<T>(Message, parameter));
    }
}
