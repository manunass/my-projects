using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IRecyclablesStore
    {
        Task<Recyclable> AddRecyclable(Recyclable recyclable);
        Task<Recyclable> GetRecyclable(string recyclableId);
        Task<List<Recyclable>> GetRecyclables();
        Task UpdateRecyclable(Recyclable recyclable);
        Task DeleteRecyclable(string recyclableId);
    }
}
