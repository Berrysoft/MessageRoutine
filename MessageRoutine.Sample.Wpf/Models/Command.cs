#nullable enable

using System;
using System.Windows.Input;

namespace MessageRoutine.Sample.Wpf.Models
{
    public class Command : ICommand
    {
#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public Action? Action { get; }

        public Command() : this(null) { }
        public Command(Action? action) => Action = action;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => Action?.Invoke();
    }
}
