using GreenCoinService.DataContracts;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IRecyclablesStore _recyclablesStore;
        private readonly IBatchesStore _batchesStore;
        private readonly IBagScansStore _bagScansStore;

        private readonly IWalletsService _walletsService;
        private readonly IUsersService _usersService;

        public OperationsService(IRecyclablesStore recyclablesStore, 
                                IBatchesStore batchesStore, 
                                IBagScansStore bagScansStore, 
                                IWalletsService walletsService,
                                IUsersService usersService)
        {
            _recyclablesStore = recyclablesStore;
            _batchesStore = batchesStore;
            _bagScansStore = bagScansStore;
            _walletsService = walletsService;
            _usersService = usersService;
        }

        public async Task<BagScan> AddBagScan(BagScan bagScan, string municipalityId, string recyclableId)
        {
            var currentBatch = await _batchesStore.GetCurrentBatch(municipalityId, recyclableId);
            InitializeBagScan(bagScan, currentBatch.Id);
            var addBagScanResult = await _bagScansStore.AddBagScan(bagScan);
            return addBagScanResult;
        }

        public async Task<BagScan> GetBagScan(string bagScanId)
        {
            var bagScan = await _bagScansStore.GetBagScan(bagScanId);
            return bagScan;
        }

        public async Task<List<BagScan>> GetBagScans(string batchId)
        {
            var bagScans = await _bagScansStore.GetBagScans(batchId);
            return bagScans;
        }

        public async Task<List<BagScan>> SearchBagScans(string userId, string employeeId, string batchId, bool? processed, long? unixTimeScannedLow, long? unixTimeScannedHigh)
        {
            var bagScans = await _bagScansStore.SearchBagScans(userId, employeeId, batchId, processed, unixTimeScannedLow, unixTimeScannedHigh);
            return bagScans;
        }

        public async Task<Recyclable> AddRecyclable(Recyclable recyclable, List<Municipality> municipalities)
        {
            var addRecyclableResult = await _recyclablesStore.AddRecyclable(recyclable);
            var tasks = new Queue<Task>();
            foreach(var municipality in municipalities)
            {
                var batch = CreateBatch(municipality.Id, addRecyclableResult.Id);
                tasks.Enqueue(_batchesStore.AddBatch(batch));
            }
            await Task.WhenAll(tasks);
            return addRecyclableResult;
        }

        public async Task DeleteRecyclable(string recyclableId)
        {
            await _recyclablesStore.DeleteRecyclable(recyclableId);
            //What do we do with the corresponding batches?
        }

        public async Task UpdateRecyclable(Recyclable recyclable)
        {
            await _recyclablesStore.UpdateRecyclable(recyclable);
        }
        public async Task<Recyclable> GetRecyclable(string recyclableId)
        {
            var recyclable = await _recyclablesStore.GetRecyclable(recyclableId);
            return recyclable;
        }

        public async Task<List<Recyclable>> GetRecyclables()
        {
            var recyclables = await _recyclablesStore.GetRecyclables();
            return recyclables;
        }

        public async Task InitializeBatches(string municipalityId)
        {
            var recyclables = await _recyclablesStore.GetRecyclables();
            var tasks = new Queue<Task>();
            foreach(var recyclable in recyclables)
            {
                var batch = CreateBatch(municipalityId, recyclable.Id);
                tasks.Enqueue(_batchesStore.AddBatch(batch));
            }
            await Task.WhenAll(tasks);
        }

        public async Task<Batch> GetBatch(string batchId)
        {
            var batch = await _batchesStore.GetBatch(batchId);
            return batch;
        }

        public async Task<Batch> GetCurrentBatch(string municipalityId, string recyclableId)
        {
            var batch = await _batchesStore.GetCurrentBatch(municipalityId, recyclableId);
            return batch;
        }

        public async Task ProcessBatch(Municipality municipality, Batch batch)
        {
            var bagScans = await _bagScansStore.GetBagScans(batch.Id);
            var coinsPerKg = CoinsPerKg(bagScans, batch.Revenue, municipality.LbpCoinRatio);
            var tasks = new Queue<Task>();
            foreach (var bagScan in bagScans)
            {
                ProcessBagScan(bagScan);
                var user = await _usersService.GetUser(bagScan.UserId);
                tasks.Enqueue(_bagScansStore.UpdateBagScan(bagScan));
                var description = Description(batch.Recyclable.Material, bagScan.Weight);
                var coins = (int)(coinsPerKg * bagScan.Weight);
                await _walletsService.UpdateWallet(batch.MunicipalityId, user.Wallet.Id, coins , description, bagScan.Id);
            }
            await Task.WhenAll(tasks);
            var newBatch = CreateSuccessorBatch(batch);
            batch.Current = false;
            await _batchesStore.UpdateBatch(batch);
            await _batchesStore.AddBatch(newBatch);
        }

        public async Task UpdateBatch(Batch batch)
        {
            await _batchesStore.UpdateBatch(batch);
        }

        private static Batch CreateBatch(string municipalityId, string recyclableId)
        {
            return new Batch
            {
                MunicipalityId = municipalityId,
                RecyclableId = recyclableId,
                BatchNumber = 0,
                Revenue = 0,
                Current = true
            };
        }

        private static Batch CreateSuccessorBatch(Batch oldBatch)
        {
            return new Batch
            {
                MunicipalityId = oldBatch.MunicipalityId,
                RecyclableId = oldBatch.RecyclableId,
                BatchNumber = oldBatch.BatchNumber + 1,
                Revenue = 0,
                Current = true
            };
        }

        private static void InitializeBagScan(BagScan bagScan, string batchId)
        {
            bagScan.BatchId = batchId;
            bagScan.Processed = false;
            bagScan.UnixTimeScanned = UnixTimeSeconds();
        }

        private static void ProcessBagScan(BagScan bagScan)
        {
            bagScan.Processed = true;
            bagScan.UnixTimeProcessed = UnixTimeSeconds();
        }

        private static double CoinsPerKg(List<BagScan> bagScans, float totalRevenue, float lbpCoinRatio)
        {
            var totalWeight = bagScans.Sum(bagScan => bagScan.Weight);
            var lbpPerKg = totalRevenue / totalWeight;
            var coinsPerKg = lbpPerKg * lbpCoinRatio;
            return coinsPerKg;
            //Another option is to multiply the LbpPerKg of a recyclable by the weight of a bag
        }

        private static string Description(string recyclable, double weight)
        {
            return "Recycling reward for " + weight.ToString() + " KGs of " + recyclable;
        }

        private static long UnixTimeSeconds()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
