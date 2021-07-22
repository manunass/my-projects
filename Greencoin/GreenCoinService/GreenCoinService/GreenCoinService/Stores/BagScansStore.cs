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
    public class BagScansStore : IBagScansStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public BagScansStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BagScan, BagScanEntity>();
                cfg.CreateMap<BagScanEntity, BagScan>();
            });
            _mapper = configuration.CreateMapper();
        }
        public async Task<BagScan> AddBagScan(BagScan bagScan)
        {
            using var context = _contextFactory.CreateDbContext();
            bagScan.Id = CreateGUID();
            var bagScanEntity = _mapper.Map<BagScanEntity>(bagScan);
            context.BagScans.Add(bagScanEntity);
            await context.SaveChangesAsync();
            return bagScan;
        }

        public async Task DeleteBagScan(string bagScanId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var bagScanEntity = new BagScanEntity
                {
                    Id = bagScanId
                };
                context.BagScans.Remove(bagScanEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Bag Scan entity with Id {bagScanId} was not found", 404);
            }
        }

        public async Task<BagScan> GetBagScan(string bagScanId)
        {
            using var context = _contextFactory.CreateDbContext();
            var bagScanEntity = await context.BagScans.Where(bagScan => bagScan.Id == bagScanId).FirstOrDefaultAsync();
            if (bagScanEntity == null)
            {
                throw new StorageErrorException($"Bag Scan entity with Id {bagScanId} was not found", 404);
            }
            var bagScan = _mapper.Map<BagScan>(bagScanEntity);
            return bagScan;
        }

        public async Task<List<BagScan>> GetBagScans(string batchId)
        {
            using var context = _contextFactory.CreateDbContext();
            var bagScanEntities = await context.BagScans.Where(bagScan => bagScan.BatchId == batchId).ToListAsync();
            var bagScans = _mapper.Map<List<BagScan>>(bagScanEntities);
            return bagScans;
        }

        public async Task<List<BagScan>> SearchBagScans(string userId, string employeeId, string batchId, bool? processed, long? unixTimeScannedLow, long? unixTimeScannedHigh)
        {
            using var context = _contextFactory.CreateDbContext();
            var bagScanEntities = await context.BagScans
                .Where(bagScan => userId == null || bagScan.UserId == userId)
                .Where(bagScan => employeeId == null || bagScan.EmployeeId == employeeId)
                .Where(bagScan => batchId == null || bagScan.BatchId == batchId)
                .Where(bagScan => processed == null || bagScan.Processed == processed)
                .Where(bagScan => unixTimeScannedLow == null || bagScan.UnixTimeScanned > unixTimeScannedLow)
                .Where(bagScan => unixTimeScannedHigh == null || bagScan.UnixTimeScanned < unixTimeScannedHigh) 
                .ToListAsync();
            var bagScans = _mapper.Map<List<BagScan>>(bagScanEntities);
            return bagScans;
        }

        public async Task UpdateBagScan(BagScan bagScan)
        {
            using var context = _contextFactory.CreateDbContext();
            var bagScanEntity = await context.BagScans
                .Where(e => e.Id == bagScan.Id)
                .FirstOrDefaultAsync();
            if (bagScanEntity == null)
            {
                throw new StorageErrorException($"Bag Scan entity with Id {bagScan.Id} was not found", 404);
            }
            _mapper.Map(bagScan, bagScanEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
