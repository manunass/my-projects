using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IMunicipalitiesStore
    {
        Task<Municipality> AddMunicipality(Municipality municipality);
        Task<Municipality> GetMunicipality(string municipalityId);
        Task UpdateMunicipality(Municipality municipality);
        Task DeleteMunicipality(string municipalityId);
        Task<List<Municipality>> GetMunicipalities();
    }
}
