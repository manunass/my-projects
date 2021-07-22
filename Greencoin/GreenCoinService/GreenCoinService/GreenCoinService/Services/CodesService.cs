using GreenCoinService.DataContracts;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class CodesService : ICodesService
    {
        private readonly ICodesStore _codesStore;

        public CodesService(ICodesStore codesStore)
        {
            _codesStore = codesStore;
        }

        public async Task<Code> CreateBusinessCode(string businessId)
        {
            var code = CreateCode(businessId);
            var addCodeResult = await _codesStore.AddCode(code);
            return addCodeResult;
        }

        public async Task<List<Code>> CreateWasteCodes(string userId, List<Recyclable> recyclables)
        {
            var tasks = new Queue<Task<Code>>();
            foreach(var recyclable in recyclables)
            {
                var code = CreateCode(userId, recyclable.Id);
                tasks.Enqueue(_codesStore.AddCode(code));
            }
            var result = await Task.WhenAll<Code>(tasks);
            return result.ToList<Code>();
        }

        public async Task<Code> GetBusinessCode(string businessId)
        {
            var businessCode = await _codesStore.GetBusinessCode(businessId);
            return businessCode;
        }

        public async Task<List<Code>> GetWasteCodes(string userId)
        {
            var wasteCodes = await _codesStore.GetWasteCodes(userId);
            return wasteCodes;
        }

        public async Task<Code> GetCode(string codeId)
        {
            var code = await _codesStore.GetCode(codeId);
            return code;
        }

        private static Code CreateCode(string businessId)
        {
            return new Code
            {
                BusinessId = businessId,
                Type = CodeType.Business
            };
        }

        private static Code CreateCode(string userId, string recyclableId)
        {
            return new Code
            {
                UserId = userId,
                RecyclableId = recyclableId,
                Type = CodeType.Waste
            };
        }
    }
}
