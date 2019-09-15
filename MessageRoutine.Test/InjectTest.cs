using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageRoutine.Test
{
    [TestClass]
    public class InjectTest
    {
        interface IConfiguration
        {
            string Data { get; }
        }

        class Configuration : IConfiguration
        {
            public string Data => "Data";
        }

        class Service
        {
            [Inject]
            protected IConfiguration? Configuration { get; set; }

            [Message("GetData")]
            public Routine GetData()
            {
                return new Routine(null, Configuration?.Data);
            }
        }

        private readonly MessageManager manager;

        public InjectTest()
        {
            manager = new MessageManager()
                .RegisterSingleton<IConfiguration, Configuration>()
                .RegisterService<Service>();
        }

        [TestMethod]
        public void DataTest()
        {
            Assert.AreEqual("Data", manager.StartRoutine("GetData"));
        }
    }
}
