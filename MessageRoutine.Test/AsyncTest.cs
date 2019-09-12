#nullable enable

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageRoutine.Test
{
    [TestClass]
    public class AsyncTest
    {
        class Service
        {
            [Message("Message1")]
            public Task<Routine> Message1Async()
            {
                return Task.FromResult(new Routine(null, 1));
            }

            [Message("Message2")]
            public ValueTask<Routine> Message2Async()
            {
                return new ValueTask<Routine>(new Routine("Message3", 2));
            }

            [Message("Message3")]
            public Routine Message3(int param)
            {
                return new Routine(null, param + 1);
            }
        }

        private readonly MessageManager manager;

        public AsyncTest()
        {
            manager = new MessageManager().RegisterService<Service>();
        }

        [TestMethod]
        public async Task TaskTest()
        {
            Assert.AreEqual(1, await manager.StartRoutineAsync("Message1"));
            Assert.AreEqual(3, await manager.StartRoutineAsync("Message2"));
        }
    }
}
