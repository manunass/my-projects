using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IRequestsService
    {
        Task<Request> AddRequest(Request request);
        Task<Request> GetRequest(string requestId);
        Task<List<Request>> SearchRequests(string municipalityId, string userId, string businessId, RequestStatus? status);
        Task ApproveRequest(string requestId);
        Task DeclineRequest(string requestId);
        Task CompleteRequest(string requestId);
        Task CancelRequest(string requestId);
        Task DeleteRequest(string requestId);
    }
}
