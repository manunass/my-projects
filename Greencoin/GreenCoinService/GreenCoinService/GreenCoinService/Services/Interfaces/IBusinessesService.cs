using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IBusinessesService
    {
        Task<Business> AddBusiness(Business business);
        Task<Business> GetBusiness(string businessId);
        Task<List<Business>> GetBusinesses(string municipalityId);
        Task<List<Business>> SearchBusinesses(string municipalityId, string category, string name);
        Task UpdateBusiness(Business business);
        Task DeleteBusiness(string businessId);
    }
}
