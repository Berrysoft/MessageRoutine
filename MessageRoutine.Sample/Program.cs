#nullable enable

using System;

namespace MessageRoutine.Sample
{
    class Program
    {
        static int Main(string[] args)
            => new MessageManager()
                .RegisterSingleton<IConfiguration, Configuration>()
                .RegisterService<Program>()
                .StartRoutine<int>("App.Main", args);

        [Inject]
        protected IConfiguration? Configuration { get; set; }

        [Message("App.Main")]
        public Routine Main()
        {
            if (Configuration != null)
                Console.WriteLine(Configuration.OutputMessage);
            return new Routine();
        }
    }
}
