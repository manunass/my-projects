using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class BusinessesStore : IBusinessesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public BusinessesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressEntity>();
                cfg.CreateMap<AddressEntity, Address>();
                cfg.CreateMap<Wallet, WalletEntity>();
                cfg.CreateMap<WalletEntity, Wallet>();
                cfg.CreateMap<Business, BusinessEntity>();
                cfg.CreateMap<BusinessEntity, Business>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Business> AddBusiness(Business business)
        {
            using var context = _contextFactory.CreateDbContext();
            var businessId = CreateGUID();
            business.Id = businessId;
            var businessEntity = _mapper.Map<BusinessEntity>(business);
            InitializeBusinessEntity(businessEntity, businessId);
            context.Businesses.Add(businessEntity);
            await context.SaveChangesAsync();
            return business;
        }

        public async Task DeleteBusiness(string businessId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var businessEntity = await context.Businesses.Where(e => e.Id == businessId)
                    .Include(e => e.Address)
                    .Include(e => e.Wallet)
                        .ThenInclude(e => e.Transactions)
                    .Include(e => e.Requests)
                    .Include(e => e.Codes)
                    .FirstOrDefaultAsync();
                context.Businesses.Remove(businessEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Business entity with Id {businessId} was not found", 404);
            }
        }

        public async Task<Business> GetBusiness(string businessId)
        {
            using var context = _contextFactory.CreateDbContext();
            var businessEntity = await context.Businesses
                .Where(e => e.Id == businessId)
                .Include(e => e.Wallet)
                .Include(e => e.Address)
                .FirstOrDefaultAsync();

            if(businessEntity == null)
            {
                throw new StorageErrorException($"Business entity with Id {businessId} was not found", 404);
            }
            var business = _mapper.Map<Business>(businessEntity);
            return business;
        }

        public async Task<List<Business>> GetBusinesses(string municipalityId)
        {
            using var context = _contextFactory.CreateDbContext();
            var businessEntities = await context.Businesses
                .Where(e => e.MunicipalityId == municipalityId)
                .Include(e => e.Wallet)
                .Include(e => e.Address)
                .ToListAsync();
            var businesses = _mapper.Map<List<Business>>(businessEntities);
            return businesses;
        }

        public async Task<List<Business>> SearchBusinesses(string municipalityId, string category, string name)
        {
            using var context = _contextFactory.CreateDbContext();
            var businessEntities = await context.Businesses
                .Where(e => e.MunicipalityId == municipalityId)
                .Where(e => category == null || e.Category == category)
                .Where(e => name == null || e.Name.Contains(name))
                .Include(e => e.Address)
                .ToListAsync();
            var businesses = _mapper.Map<List<Business>>(businessEntities);
            return businesses;
        }

        public async Task UpdateBusiness(Business business)
        {
            using var context = _contextFactory.CreateDbContext();
            var businessEntity = await context.Businesses
                .Where(e => e.Id == business.Id)
                .Include(e => e.Address)
                .Include(e => e.Wallet)
                .FirstOrDefaultAsync();
            if (businessEntity == null)
            {
                throw new StorageErrorException($"Business entity with Id {business.Id} was not found", 404);
            }
            _mapper.Map(business, businessEntity);
            await context.SaveChangesAsync();
        }

        private static void InitializeBusinessEntity(BusinessEntity businessEntity, string businessId)
        {
            businessEntity.Address.Id = CreateGUID();
            businessEntity.Address.BusinessId = businessId;
            businessEntity.Wallet.Id = CreateGUID();
            businessEntity.Wallet.BusinessId = businessId;
        }

        private static string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
