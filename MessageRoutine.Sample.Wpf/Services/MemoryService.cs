#nullable enable

using System.Threading.Tasks;

namespace MessageRoutine.Sample.Wpf.Services
{
    class MemoryService
    {
        [Message(Program.TextServiceGetMessage)]
        public Task<Routine> GetTextAsync()
        {
            return Task.FromResult(new Routine(null, "Hello world!"));
        }
    }
}
