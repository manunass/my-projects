using GreenCoinService.DataContracts;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class MunicipalitiesService: IMunicipalitiesService
    {
        private readonly IMunicipalitiesStore _municipalitiesStore;
        private readonly IEmployeesStore _employeesStore;
        private readonly IOperationsService _operationsService;

        public MunicipalitiesService(IMunicipalitiesStore municipalitiesStore, IEmployeesStore employeesStore, IOperationsService operationsService)
        {
            _municipalitiesStore = municipalitiesStore;
            _employeesStore = employeesStore;
            _operationsService = operationsService;
        }

        public async Task<Municipality> AddMunicipality(Municipality municipality)
        {
            var addMunicipalityResult = await _municipalitiesStore.AddMunicipality(municipality);
            await _operationsService.InitializeBatches(addMunicipalityResult.Id);
            return addMunicipalityResult;
        }

        public async Task DeleteMunicipality(string municipalityId)
        {
            await _municipalitiesStore.DeleteMunicipality(municipalityId);
        }

        public async Task<List<Municipality>> GetMunicipalities()
        {
            var municipalities = await _municipalitiesStore.GetMunicipalities();
            return municipalities;
        }

        public async Task<Municipality> GetMunicipality(string municipalityId)
        {
            var municipality = await _municipalitiesStore.GetMunicipality(municipalityId);
            return municipality;
        }

        public async Task UpdateMunicipality(Municipality municipality)
        {
            await _municipalitiesStore.UpdateMunicipality(municipality);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var addEmployeeResult = await _employeesStore.AddEmployee(employee);
            return addEmployeeResult;
        }

        public async Task DeleteEmployee(string employeeId)
        {
            await _employeesStore.DeleteEmployee(employeeId);
        }

        public async Task<Employee> GetEmployee(string employeeId)
        {
            var employee = await _employeesStore.GetEmployee(employeeId);
            return employee;
        }

        public async Task<List<Employee>> GetEmployees(string municipalityId)
        {
            var employees = await _employeesStore.GetEmployees(municipalityId);
            return employees;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await _employeesStore.UpdateEmployee(employee);
        }


    }
}
