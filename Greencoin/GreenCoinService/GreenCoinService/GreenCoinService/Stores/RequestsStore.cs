using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class RequestsStore : IRequestsStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public RequestsStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Request, RequestEntity>();
                cfg.CreateMap<RequestEntity, Request>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Request> AddRequest(Request request)
        {
            using var context = _contextFactory.CreateDbContext();
            request.Id = CreateGUID();
            var requestEntity = _mapper.Map<RequestEntity>(request);
            context.Requests.Add(requestEntity);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task DeleteRequest(string requestId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var requestEntity = new RequestEntity
                {
                    Id = requestId
                };
                context.Requests.Remove(requestEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Request entity with Id {requestId} was not found", 404);
            }
        }

        public async Task<Request> GetRequest(string requestId)
        {
            using var context = _contextFactory.CreateDbContext();
            var requestEntity = await context.Requests.Where(request => request.Id == requestId).FirstOrDefaultAsync();
            if (requestEntity == null)
            {
                throw new StorageErrorException($"Request entity with Id {requestId} was not found", 404);
            }
            var request = _mapper.Map<Request>(requestEntity);
            return request;
        }

        public async Task<List<Request>> SearchRequests(string municipalityId, string userId, string businessId, RequestStatus? status)
        {
            using var context = _contextFactory.CreateDbContext();
            var requestEntities = await context.Requests
                .Where(request => municipalityId == null || (request.User == null || request.User.MunicipalityId == municipalityId))
                .Where(request => municipalityId == null || (request.Business == null || request.Business.MunicipalityId == municipalityId))
                .Where(request => userId == null || request.UserId == userId)
                .Where(request => businessId == null || request.BusinessId == businessId)
                .Where(request => status == null || request.Status == status.ToString())
                .ToListAsync();
            var requests = _mapper.Map<List<Request>>(requestEntities);
            return requests;
        }

        public async Task UpdateRequest(Request request)
        {
            using var context = _contextFactory.CreateDbContext();
            var requestEntity = await context.Requests
                .Where(e => e.Id == request.Id)
                .FirstOrDefaultAsync();
            if (requestEntity == null)
            {
                throw new StorageErrorException($"Request entity with Id {request.Id} was not found", 404);
            }
            _mapper.Map(request, requestEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
