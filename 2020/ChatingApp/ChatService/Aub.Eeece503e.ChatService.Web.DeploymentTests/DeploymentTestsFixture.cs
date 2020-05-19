using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Web.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eeece503e.ChatService.Web.DeploymentTests
{
    public class DeploymentTestsFixture : IEndToEndTestsFixture
    {
        public DeploymentTestsFixture()
        {
            string serviceUrl = Environment.GetEnvironmentVariable("ChatServiceDeploymentTestsUrl");
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new Exception("Could not find ChatServiceUrl environment variable");
            }

            ChatServiceClient = new ChatServiceClient(new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri(serviceUrl)
            });
        }
        public IChatServiceClient ChatServiceClient { get; }
    }
}
