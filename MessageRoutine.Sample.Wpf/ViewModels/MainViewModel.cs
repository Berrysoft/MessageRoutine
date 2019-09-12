#nullable enable

using System.ComponentModel;
using System.Windows.Input;
using MessageRoutine.Sample.Wpf.Models;
using MessageRoutine.Sample.Wpf.Services;

namespace MessageRoutine.Sample.Wpf.ViewModels
{
    public enum MainServiceType
    {
        Memory,
        File,
        Web
    }

    public class MainViewModel : INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        public MainViewModel()
        {
            GetTextCommand = new Command(async () =>
            {
                Text = await Program.Manager.StartRoutineAsync<string?>(Program.TextServiceGetMessage);
            });
        }

        public MainServiceType ServiceType { get; set; }
        private void OnServiceTypeChanged()
        {
            switch (ServiceType)
            {
                case MainServiceType.Memory:
                    ReregisterService<MemoryService>();
                    break;
                case MainServiceType.File:
                    ReregisterService<FileService>();
                    break;
                case MainServiceType.Web:
                    ReregisterService<WebService>();
                    break;
            }
        }
        private MessageManager ReregisterService<T>() => Program.Manager.UnregisterService<T>().RegisterService<T>();

        public string? Text { get; set; }

        public ICommand GetTextCommand { get; }
    }
}
