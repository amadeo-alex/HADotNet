using HADotNet.Core.Clients;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HADotNet.Core.Tests
{
    public class ServiceTests
    {
        private Uri Instance { get; set; }
        private string ApiKey { get; set; }

        [SetUp]
        public void Setup()
        {
            Instance = new Uri(Environment.GetEnvironmentVariable("HADotNet:Tests:Instance"));
            ApiKey = Environment.GetEnvironmentVariable("HADotNet:Tests:ApiKey");

            ClientFactory.Initialize(Instance, ApiKey);
        }

        [Test]
        public async Task ShouldRetrieveServiceList()
        {
            var client = ClientFactory.GetClient<ServiceClient>();

            var services = await client.GetServices();

            Assert.IsNotNull(services);
            Assert.AreNotEqual(0, services.Count);
        }

        [Test]
        public async Task ShouldCallService()
        {
            var client = ClientFactory.GetClient<ServiceClient>();

            var returnState = await client.CallService("light.turn_on", new { entity_id = "light.sample_light" });

            Assert.IsNotNull(returnState);

            // Uncomment if you are actually running a test against a real entity that will return state data
            //Assert.AreNotEqual(0, returnState.Count);
        }

        [Test]
        public async Task ShouldCallServiceWithEntityId()
        {
            var client = ClientFactory.GetClient<ServiceClient>();

            var returnState = await client.CallServiceForEntities("light.turn_on", "light.my_light_1");

            Assert.IsNotNull(returnState);

            // Uncomment if you are actually running a test against a real entity that will return state data
            //Assert.AreNotEqual(0, returnState.Count);
        }

        [Test]
        public async Task ShouldCallServiceWithEntityIds()
        {
            var client = ClientFactory.GetClient<ServiceClient>();

            var returnState = await client.CallServiceForEntities("light.turn_on", "light.my_light_1", "light.my_light_2");

            Assert.IsNotNull(returnState);

            // Uncomment if you are actually running a test against a real entity that will return state data
            //Assert.AreNotEqual(0, returnState.Count);
        }

        [Test]
        public async Task ShouldCallServiceToTriggerAutomation()
        {
            var client = ClientFactory.GetClient<ServiceClient>();

            var returnState = await client.CallServiceForEntities("automation.trigger", "automation.test_automation");

            Assert.IsNotNull(returnState);
        }
    }
}