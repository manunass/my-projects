using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IRequestsStore
    {
        Task<Request> AddRequest(Request request);
        Task<Request> GetRequest(string requestId);
        Task<List<Request>> SearchRequests(string municipalityId, string userId, string businessId, RequestStatus? status);
        Task UpdateRequest(Request request);
        Task DeleteRequest(string requestId);
    }
}
