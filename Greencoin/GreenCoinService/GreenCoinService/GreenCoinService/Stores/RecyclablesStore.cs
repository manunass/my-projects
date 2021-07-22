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
    public class RecyclablesStore : IRecyclablesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public RecyclablesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Recyclable, RecyclableEntity>();
                cfg.CreateMap<RecyclableEntity, Recyclable>();
            });
            _mapper = configuration.CreateMapper();
        }
        public async Task<Recyclable> AddRecyclable(Recyclable recyclable)
        {
            using var context = _contextFactory.CreateDbContext();
            recyclable.Id = CreateGUID();
            var recyclableEntity = _mapper.Map<RecyclableEntity>(recyclable);
            context.Recyclables.Add(recyclableEntity);
            await context.SaveChangesAsync();
            return recyclable;
        }

        public async Task DeleteRecyclable(string recyclableId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var recyclableEntity = new RecyclableEntity
                {
                    Id = recyclableId
                };
                context.Recyclables.Remove(recyclableEntity);
                await context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new StorageErrorException($"Recyclable entity with Id {e} was not found", 404);
            }
        }

        public async Task<Recyclable> GetRecyclable(string recyclableId)
        {
            using var context = _contextFactory.CreateDbContext();
            var recyclableEntity = await context.Recyclables.Where(recyclable => recyclable.Id == recyclableId).FirstOrDefaultAsync();
            if (recyclableEntity == null)
            {
                throw new StorageErrorException($"Recyclable entity with Id {recyclableId} was not found", 404);
            }
            var recyclable = _mapper.Map<Recyclable>(recyclableEntity);
            return recyclable;
        }

        public async Task<List<Recyclable>> GetRecyclables()
        {
            using var context = _contextFactory.CreateDbContext();
            var recyclableEntities = await context.Recyclables.ToListAsync();
            var recyclables = _mapper.Map<List<Recyclable>>(recyclableEntities);
            return recyclables;
        }

        public async Task UpdateRecyclable(Recyclable recyclable)
        {
            using var context = _contextFactory.CreateDbContext();
            var recyclableEntity = await context.Recyclables
                .Where(e => e.Id == recyclable.Id)
                .FirstOrDefaultAsync();
            if (recyclableEntity == null)
            {
                throw new StorageErrorException($"Recyclable entity with Id {recyclable.Id} was not found", 404);
            }
            _mapper.Map(recyclable, recyclableEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
