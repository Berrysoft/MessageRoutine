#nullable enable

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageRoutine.Test
{
    [TestClass]
    public class BasicTest
    {
        class Service
        {
            [Message("Message0")]
            public Routine Message0()
            {
                return new Routine();
            }

            [Message("Message1")]
            public Routine Message1()
            {
                return new Routine(null, 1);
            }

            [Message("Message2")]
            public Routine Message2()
            {
                return new Routine("Message3", 2);
            }

            [Message("Message3")]
            public Routine Message3(int param)
            {
                return new Routine(null, param + 1);
            }

            [Message("Message4")]
            [Message("Message5")]
            public Routine Message4Or5(int param, [FromMessage] string message)
            {
                return new Routine(null, param + (message == "Message4" ? 4 : 5));
            }
        }

        private readonly MessageManager manager;

        public BasicTest()
        {
            manager = new MessageManager().RegisterService<Service>();
        }

        [TestMethod]
        public void InvokeTest()
        {
            Assert.AreEqual(0, manager.StartRoutine<int>("Message0"));
            Assert.AreEqual(null, manager.StartRoutine<int?>("Message0"));
            Assert.AreEqual(null, manager.StartRoutine("Message0"));
            Assert.AreEqual(1, manager.StartRoutine("Message1"));
            Assert.AreEqual(3, manager.StartRoutine("Message2"));
        }

        [TestMethod]
        public void NoMessageTest()
        {
            Assert.AreEqual(10086, manager.StartRoutine(null, 10086));
            Assert.AreEqual(10086, manager.StartRoutine("ChinaMobile", 10086));
        }

        [TestMethod]
        public void MultipleTest()
        {
            Assert.AreEqual(4, manager.StartRoutine("Message4", 0));
            Assert.AreEqual(5, manager.StartRoutine("Message5", 0));
        }

        class ChinaMobileService
        {
            [Message("ChinaMobileCall")]
            public Routine Call1(int number)
            {
                return new Routine(null, 10000 + number);
            }
        }

        [TestMethod]
        public void AddServiceTest()
        {
            Assert.AreEqual(86, manager.StartRoutine("ChinaMobileCall", 86));
            manager.RegisterService<ChinaMobileService>();
            Assert.AreEqual(10086, manager.StartRoutine("ChinaMobileCall", 86));
            manager.UnregisterService<ChinaMobileService>();
            Assert.AreEqual(86, manager.StartRoutine("ChinaMobileCall", 86));
        }

        class AdvancedService
        {
            [Message("Message3")]
            public Routine Message3(int param)
            {
                return new Routine("Message3", typeof(Service), param + 1);
            }
        }

        [TestMethod]
        public void OverrideServiceTest()
        {
            Assert.AreEqual(3, manager.StartRoutine("Message2"));
            manager.RegisterService<AdvancedService>();
            Assert.AreEqual(4, manager.StartRoutine("Message2"));
            manager.UnregisterService<AdvancedService>();
            Assert.AreEqual(3, manager.StartRoutine("Message2"));
        }
    }
}
