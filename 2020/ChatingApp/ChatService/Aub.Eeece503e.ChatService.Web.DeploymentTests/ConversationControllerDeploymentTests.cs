using Aub.Eece503e.ChatService.Web.IntegrationTests;

namespace Aub.Eeece503e.ChatService.Web.DeploymentTests
{
    public class ConversationControllerDeploymentTests : ConversationControllerEndToEndTests<DeploymentTestsFixture>
    {
        public ConversationControllerDeploymentTests(DeploymentTestsFixture fixture) : base(fixture)
        {
        }
    }
}
