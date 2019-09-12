#nullable enable

using System.IO;
using System.Threading.Tasks;

namespace MessageRoutine.Sample.Wpf.Services
{
    class FileService
    {
        [Message(Program.TextServiceGetMessage)]
        public async Task<Routine> GetTextAsync()
        {
            using StreamReader reader = new StreamReader("DataFile.txt");
            return new Routine(null, await reader.ReadToEndAsync());
        }
    }
}
