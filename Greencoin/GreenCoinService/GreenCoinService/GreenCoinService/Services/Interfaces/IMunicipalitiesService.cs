using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services.Interfaces
{
    public interface IMunicipalitiesService
    {
        Task<Municipality> AddMunicipality(Municipality municipality);
        Task<Municipality> GetMunicipality(string municipalityId);
        Task UpdateMunicipality(Municipality municipality);
        Task DeleteMunicipality(string municipalityId);
        Task<List<Municipality>> GetMunicipalities();
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> GetEmployee(string employeeId);
        Task<List<Employee>> GetEmployees(string municipalityId);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(string employeeId);
    }
}
