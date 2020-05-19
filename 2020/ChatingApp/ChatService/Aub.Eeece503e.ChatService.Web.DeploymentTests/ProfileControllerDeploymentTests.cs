using Aub.Eece503e.ChatService.Web.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eeece503e.ChatService.Web.DeploymentTests
{
    public class ProfileControllerDeploymentTests : ProfileControllerEndToEndTests<DeploymentTestsFixture>
    {
        public ProfileControllerDeploymentTests(DeploymentTestsFixture fixture) : base(fixture)
        {
        }
    }
}
