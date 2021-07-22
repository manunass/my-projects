using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface ICodesStore
    {
        Task<Code> AddCode(Code code);
        Task<Code> GetCode(string codeId);
        Task<List<Code>> GetWasteCodes(string userId);
        Task<Code> GetBusinessCode(string businessId);
        Task UpdateCode(Code code);
        Task DeleteCode(string codeId);
    }
}
