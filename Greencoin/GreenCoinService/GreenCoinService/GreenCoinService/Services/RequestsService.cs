using GreenCoinService.DataContracts;
using GreenCoinService.Exceptions;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class RequestsService : IRequestsService
    {
        private readonly IRequestsStore _requestsStore;

        private readonly IMunicipalitiesService _municipalitiesService;
        private readonly IUsersService _usersService;
        private readonly IBusinessesService _businessesService;
        private readonly IWalletsService _walletsService;

        public RequestsService(IRequestsStore requestsStore,
                            IMunicipalitiesService municipalitiesService,
                            IUsersService usersService,
                            IWalletsService walletsService,
                            IBusinessesService businessesService)
        {
            _requestsStore = requestsStore;
            _municipalitiesService = municipalitiesService;
            _usersService = usersService;
            _walletsService = walletsService;
            _businessesService = businessesService;
        }
        public async Task<Request> AddRequest(Request request)
        {
            var owner = await GetRequestOwner(request);
            var municipality = await _municipalitiesService.GetMunicipality(owner.MunicipalityId);
            var wallet = await _walletsService.GetWalletByOwner(owner.Id);
            CheckIsValidBalanceOrThrow(wallet.Balance, municipality.CoinsCashoutTreshold);
            InitializeRequest(request);
            var addRequestResult = await _requestsStore.AddRequest(request);
            return addRequestResult;
        }

        public async Task ApproveRequest(string requestId)
        {
            var request = await _requestsStore.GetRequest(requestId);
            CheckIsValidStatusOrThrow(RequestStatus.Pending, request.Status, RequestStatus.Approved);
            ApproveRequest(request);
            await _requestsStore.UpdateRequest(request);
        }

        public async Task CancelRequest(string requestId)
        {
            var request = await _requestsStore.GetRequest(requestId);
            CheckIsValidStatusOrThrow(RequestStatus.Pending, RequestStatus.Approved, request.Status, RequestStatus.Cancelled);
            request.Status = RequestStatus.Cancelled;
            await _requestsStore.UpdateRequest(request);
        }

        public async Task CompleteRequest(string requestId)
        {
            var request = await _requestsStore.GetRequest(requestId);
            CheckIsValidStatusOrThrow(RequestStatus.Approved, request.Status, RequestStatus.Completed);
            var owner = await GetRequestOwner(request);
            var municipality = await _municipalitiesService.GetMunicipality(owner.MunicipalityId);
            var wallet = await _walletsService.GetWalletByOwner(owner.Id);            
            await _walletsService.UpdateWallet(municipality.Id, wallet.Id, -wallet.Balance, $"Cashout: {wallet.Balance} coins");
            CompleteRequest(request);
            await _requestsStore.UpdateRequest(request);
        }

        public async Task DeclineRequest(string requestId)
        {
            var request = await _requestsStore.GetRequest(requestId);
            CheckIsValidStatusOrThrow(RequestStatus.Pending, request.Status, RequestStatus.Declined);
            request.Status = RequestStatus.Declined;
            await _requestsStore.UpdateRequest(request);
        }

        public async Task DeleteRequest(string requestId)
        {
            await _requestsStore.DeleteRequest(requestId);
        }

        public async Task<Request> GetRequest(string requestId)
        {
            var request = await _requestsStore.GetRequest(requestId);
            return request;
        }

        public async Task<List<Request>> SearchRequests(string municipalityId, string userId, string businessId, RequestStatus? status)
        {
            var requests = await _requestsStore.SearchRequests(municipalityId, userId, businessId, status);
            return requests;
        }

        private async Task<IPlatformUser> GetRequestOwner(Request request)
        {
            IPlatformUser owner;
            if (request.BusinessId != null)
            {
                owner = await _businessesService.GetBusiness(request.BusinessId);
            }
            else
            {
                owner = await _usersService.GetUser(request.UserId);
            }
            return owner;
        }

        private static void InitializeRequest(Request request)
        {
            request.Status = RequestStatus.Pending;
            request.UnixTimeRequested = UnixTimeSeconds();
        }

        private static void ApproveRequest(Request request)
        {
            request.Status = RequestStatus.Approved;
            request.UnixTimeApproved = UnixTimeSeconds();
        }

        private static void CompleteRequest(Request request)
        {
            request.Status = RequestStatus.Completed;
            request.UnixTimeCompleted = UnixTimeSeconds();
        }



        private static void CheckIsValidStatusOrThrow(RequestStatus expectedStatus, RequestStatus actualStatus, RequestStatus targetStatus)
        {
            if(expectedStatus != actualStatus)
            {
                throw new BadRequestException($"Cannot set '{actualStatus}' request to '{targetStatus}'. The request must be '{expectedStatus}'.");
            }
        }

        private static void CheckIsValidStatusOrThrow(RequestStatus expectedStatus1, RequestStatus expectedStatus2, RequestStatus actualStatus, RequestStatus targetStatus)
        {
            if (expectedStatus1 != actualStatus && expectedStatus2 != actualStatus)
            {
                throw new BadRequestException($"Cannot set '{actualStatus}' request to '{targetStatus}'. The request must be '{expectedStatus1}' or '{expectedStatus2}'.");
            }
        }

        private static void CheckIsValidBalanceOrThrow(int balance, int coinsCheckoutThreshold)
        {
            if(balance < coinsCheckoutThreshold)
            {
                throw new BadRequestException($"Cannot request a cashout with a balance less than {coinsCheckoutThreshold} coins.");
            }
        }

        private static long UnixTimeSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
