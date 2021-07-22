using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class BatchesStore : IBatchesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public BatchesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Batch, BatchEntity>();
                cfg.CreateMap<BatchEntity, Batch>();
                cfg.CreateMap<RecyclableEntity, Recyclable>();
                cfg.CreateMap<Recyclable, RecyclableEntity>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Batch> AddBatch(Batch batch)
        {
            using var context = _contextFactory.CreateDbContext();
            batch.Id = CreateGUID();
            var batchEntity = _mapper.Map<BatchEntity>(batch);
            context.Batches.Add(batchEntity);
            await context.SaveChangesAsync();
            return batch;
        }

        public async Task DeleteBatch(string batchId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var batchEntity = new BatchEntity
                {
                    Id = batchId
                };
                context.Batches.Remove(batchEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Batch entity with Id {batchId} was not found", 404);
            }
        }

        public async Task<Batch> GetBatch(string batchId)
        {
            using var context = _contextFactory.CreateDbContext();
            var batchEntity = await context.Batches
                .Include(batch => batch.Recyclable)
                .Where(batch => batch.Id == batchId)
                .FirstOrDefaultAsync();
            if (batchEntity == null)
            {
                throw new StorageErrorException($"Batch entity with Id {batchId} was not found", 404);
            }
            var batch = _mapper.Map<Batch>(batchEntity);
            return batch;
        }

        public async Task<Batch> GetCurrentBatch(string municipalityId, string recyclableId)
        {
            using var context = _contextFactory.CreateDbContext();
            var batchEntity = await context.Batches
                .Include(batch => batch.Recyclable)
                .Where(batch =>
                    batch.MunicipalityId == municipalityId
                    && batch.RecyclableId == recyclableId
                    && batch.Current == true)
                .FirstOrDefaultAsync();
            if (batchEntity == null)
            {
                throw new StorageErrorException($"Current Batch entity with Municipality Id {municipalityId} and Recyclable Id {recyclableId} was not found", 404);
            }
            var batch = _mapper.Map<Batch>(batchEntity);
            return batch;
        }

        public async Task UpdateBatch(Batch batch)
        {
            batch.Recyclable = null;
            using var context = _contextFactory.CreateDbContext();
            var batchEntity = await context.Batches
                .Where(e => e.Id == batch.Id)
                .FirstOrDefaultAsync();
            if (batchEntity == null)
            {
                throw new StorageErrorException($"Batch entity with Id {batch.Id} was not found", 404);
            }
            _mapper.Map(batch, batchEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
