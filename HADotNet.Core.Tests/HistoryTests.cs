using HADotNet.Core;
using HADotNet.Core.Clients;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace HADotNet.Core.Tests
{
    public class HistoryTests
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
        public async Task ShouldRetrieveHistoryByEntityId()
        {
            var client = ClientFactory.GetClient<HistoryClient>();

            var history = await client.GetHistory("sun.sun");

            Assert.IsNotNull(history);
            Assert.IsNotEmpty(history.EntityId);
            Assert.AreNotEqual(0, history.Count);
        }

        [Test]
        public async Task ShouldRetrieveHistoryByStartDate()
        {
            var client = ClientFactory.GetClient<HistoryClient>();

            var allHistory = await client.GetHistory("sun.sun", DateTimeOffset.Now.Subtract(TimeSpan.FromDays(2)), DateTime.Now);

            Assert.IsNotNull(allHistory);
            Assert.IsNotEmpty(allHistory[0].EntityId);
            Assert.AreNotEqual(0, allHistory.Count);
        }

        [Test]
        public async Task ShouldRetrieveHistoryByStartAndEndDateAndEntityId()
        {
            var client = ClientFactory.GetClient<HistoryClient>();

            var history = await client.GetHistory("sun.sun", DateTimeOffset.Now.Subtract(TimeSpan.FromDays(2)), DateTime.Now);

            Assert.IsNotNull(history);
            Assert.IsNotEmpty(history.EntityId);
            Assert.AreNotEqual(0, history.Count);
        }
    }
}