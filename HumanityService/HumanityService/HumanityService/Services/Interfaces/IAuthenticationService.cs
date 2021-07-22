using HumanityService.DataContracts;
using HumanityService.DataContracts.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumanityService.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginUser(string username, string password);

        Task<AuthenticationResult> LoginNgo(string username, string password);
    }
}
