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
    public class MunicipalitiesStore : IMunicipalitiesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public MunicipalitiesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, AddressEntity>();
                cfg.CreateMap<AddressEntity, Address>();
                cfg.CreateMap<Municipality, MunicipalityEntity>();
                cfg.CreateMap<MunicipalityEntity, Municipality>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Municipality> AddMunicipality(Municipality municipality)
        {
            using var context = _contextFactory.CreateDbContext();
            var municipalityId = CreateGUID();
            municipality.Id = municipalityId;
            var municipalityEntity = _mapper.Map<MunicipalityEntity>(municipality);
            InitializeMunicipalityEntity(municipalityEntity, municipalityId);
            context.Municipalities.Add(municipalityEntity);
            await context.SaveChangesAsync();
            return municipality;
        }

        public async Task DeleteMunicipality(string municipalityId)
        {
            //The database will cascade-delete: Address, Employees, Batches
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var municipalityEntity = await context.Municipalities.Where(e => e.Id == municipalityId)
                    .Include(e => e.Address)
                    .Include(e => e.Employees)
                    .FirstOrDefaultAsync();
                context.Municipalities.Remove(municipalityEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Municipality entity with Id {municipalityId} was not found", 404);
            }
        }

        public async Task<List<Municipality>> GetMunicipalities()
        {
            using var context = _contextFactory.CreateDbContext();
            var Municipalities = await context.Municipalities
                .Include(municipality => municipality.Address)
                .ToListAsync();
            var municipalities = _mapper.Map<List<MunicipalityEntity>, List<Municipality>>(Municipalities);
            return municipalities;
        }

        public async Task<Municipality> GetMunicipality(string municipalityId)
        {
            using var context = _contextFactory.CreateDbContext();
            var municipalityEntity = await context.Municipalities
                .Where(municipality => municipality.Id == municipalityId)
                .Include(municipality => municipality.Address)
                .FirstOrDefaultAsync();

            if (municipalityEntity == null)
            {
                throw new StorageErrorException($"Municipality entity with Id {municipalityId} was not found", 404);
            }
            var municipality = _mapper.Map<MunicipalityEntity, Municipality>(municipalityEntity);
            return municipality;
        }

        public async Task UpdateMunicipality(Municipality municipality)
        {
            using var context = _contextFactory.CreateDbContext();
            var municipalityEntity = await context.Municipalities
                .Where(e => e.Id == municipality.Id)
                .Include(municipality => municipality.Address)
                .FirstOrDefaultAsync();
            if (municipalityEntity == null)
            {
                throw new StorageErrorException($"Municipality entity with Id {municipality.Id} was not found", 404);
            }
            _mapper.Map(municipality, municipalityEntity);
            await context.SaveChangesAsync();
        }

        private static void InitializeMunicipalityEntity(MunicipalityEntity municipalityEntity, string municipalityId)
        {
            municipalityEntity.Address.MunicipalityId = municipalityId;
            municipalityEntity.Address.Id = CreateGUID();
        }

        private static string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
