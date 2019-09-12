#nullable enable

using System.Net.Http;
using System.Threading.Tasks;

namespace MessageRoutine.Sample.Wpf.Services
{
    class WebService
    {
        [Inject]
        protected HttpClient? Http { get; set; }

        [Message(Program.TextServiceGetMessage)]
        public async Task<Routine> GetTextAsync()
        {
            return new Routine(null, await Http!.GetStringAsync("http://www.bing.com/"));
        }
    }
}
