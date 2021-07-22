using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<Recyclable> AddRecyclable(Recyclable recyclable, List<Municipality> municipalities);
        Task<Recyclable> GetRecyclable(string recyclableId);
        Task<List<Recyclable>> GetRecyclables();
        Task UpdateRecyclable(Recyclable recyclable);
        Task DeleteRecyclable(string recyclableId);
        Task InitializeBatches(string municipalityId);
        Task<Batch> GetBatch(string batchId);
        Task<Batch> GetCurrentBatch(string municipalityId, string recyclableId);
        Task ProcessBatch(Municipality municipality, Batch batch);
        Task UpdateBatch(Batch batch);
        Task<BagScan> AddBagScan(BagScan bagScan, string municipalityId, string recyclableId);
        Task<BagScan> GetBagScan(string bagScanId);
        Task<List<BagScan>> SearchBagScans(string userId, string employeeId, string batchId, bool? processed, long? unixTimeScannedLow, long? unixTimeScannedHigh);
        Task<List<BagScan>> GetBagScans(string batchId);
    }
}
