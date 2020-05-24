using Aub.Eece503e.ChatService.Web.IntegrationTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aub.Eeece503e.ChatService.Web.DeploymentTests
{
    public class ImageControllerDeploymentTests : ImageControllerEndToEndTests<DeploymentTestsFixture>
    {
        public ImageControllerDeploymentTests(DeploymentTestsFixture fixture) : base(fixture)
        {
        }
    }
}
