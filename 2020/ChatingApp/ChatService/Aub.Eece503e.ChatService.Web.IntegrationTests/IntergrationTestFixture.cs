using Aub.Eece503e.ChatService.Client;
using Aub.Eece503e.ChatService.Web.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Aub.Eece503e.ChatService.Web.IntegrationTests
{
    public class IntegrationTestFixture : IEndToEndTestsFixture
    {
        public IntegrationTestFixture()
        {
            TestServer testServer = new TestServer(Program.CreateWebHostBuilder(new string[] { }).UseEnvironment("Development"));
            var httpClient = testServer.CreateClient();
            ChatServiceClient = new ChatServiceClient(httpClient);
            ConversationStore = testServer.Host.Services.GetRequiredService<IConversationStore>();

        }

        public IChatServiceClient ChatServiceClient { get; }
        public IConversationStore ConversationStore { get; }
    }
}
