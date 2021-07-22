using GreenCoinService.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface ICodesService
    {
        Task<Code> CreateBusinessCode(string businessId);
        Task<List<Code>> CreateWasteCodes(string userId, List<Recyclable> recyclables);
        Task<Code> GetBusinessCode(string businessId);
        Task<List<Code>> GetWasteCodes(string userId);
        Task<Code> GetCode(string codeId);
    }
}
