using Aub.Eece503e.ChatService.Client;

namespace Aub.Eece503e.ChatService.Web.IntegrationTests
{
    public interface IEndToEndTestsFixture
    {
        public IChatServiceClient ChatServiceClient { get; }
    }
}
