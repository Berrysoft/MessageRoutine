using System;

namespace MessageRoutine.Sample.Wpf
{
    public class Program
    {
        public static readonly MessageManager Manager = new MessageManager();

        [STAThread]
        public static int Main() => Manager
            .RegisterSingleton<App>()
            .RegisterService<Program>()
            .StartRoutine<int>("Program.Startup");

        [Inject]
        protected App App { get; set; }

        [Message("Program.Startup")]
        public Routine Startup() => new Routine(null, App.Run());
    }
}
