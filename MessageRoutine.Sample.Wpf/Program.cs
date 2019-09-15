using System;
using System.Net.Http;
using MessageRoutine.Sample.Wpf.Services;

namespace MessageRoutine.Sample.Wpf
{
    public class Program
    {
        public static readonly MessageManager Manager = new MessageManager();

        public const string ProgramStartupMessage = "Program.Startup";
        public const string TextServiceGetMessage = "TextService.Get";

        [STAThread]
        public static int Main() => Manager
            .RegisterSingleton<App>()
            .RegisterSingleton<HttpClient>()
            .RegisterService<Program>()
            .RegisterService<MemoryService>()
            .StartRoutine<int>(ProgramStartupMessage);

        [Inject]
        protected App? App { get; set; }

        [Message(ProgramStartupMessage)]
        public Routine Startup() => new Routine(null, App!.Run());
    }
}
