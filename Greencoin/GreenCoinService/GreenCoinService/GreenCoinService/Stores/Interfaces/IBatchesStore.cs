using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IBatchesStore
    {
        Task<Batch> AddBatch(Batch batch);
        Task<Batch> GetBatch(string batchId);
        Task<Batch> GetCurrentBatch(string municipalityId, string recyclableId);
        Task UpdateBatch(Batch batch);
        Task DeleteBatch(string batchId);
    }
}
