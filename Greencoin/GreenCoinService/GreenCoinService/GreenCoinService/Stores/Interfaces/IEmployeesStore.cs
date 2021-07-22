using GreenCoinService.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores.Interfaces
{
    public interface IEmployeesStore
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> GetEmployee(string employeeId);
        Task<List<Employee>> GetEmployees(string municipalityId);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(string employeeId);
    }
}
