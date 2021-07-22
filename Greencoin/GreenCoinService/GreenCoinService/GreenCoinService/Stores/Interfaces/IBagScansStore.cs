using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IBagScansStore
    {
        Task<BagScan> AddBagScan(BagScan bagScan);
        Task<BagScan> GetBagScan(string bagScanId);
        Task<List<BagScan>> GetBagScans(string batchId);
        Task<List<BagScan>> SearchBagScans(string userId, string employeeId, string batchId, bool? processed, long? unixTimeScannedLow, long? unixTimeScannedHigh);
        Task UpdateBagScan(BagScan bagScan);
        Task DeleteBagScan(string bagScanId);
    }
}
