#nullable enable

namespace MessageRoutine.Sample
{
    interface IConfiguration
    {
        string OutputMessage { get; }
    }

    class Configuration : IConfiguration
    {
        public string OutputMessage => "Hello world!";
    }
}
