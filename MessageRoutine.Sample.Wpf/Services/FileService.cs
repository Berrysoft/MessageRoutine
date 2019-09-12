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
            return new Routine(null, await File.ReadAllTextAsync("DataFile.txt"));
        }
    }
}
