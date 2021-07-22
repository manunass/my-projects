using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Entities;
using GreenCoinService.Exceptions;
using GreenCoinService.Migrations;
using GreenCoinService.Stores.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Stores
{
    public class EmployeesStore : IEmployeesStore
    {
        private readonly IDbContextFactory<GreenCoinServiceDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public EmployeesStore(IDbContextFactory<GreenCoinServiceDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeEntity>();
                cfg.CreateMap<EmployeeEntity, Employee>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            using var context = _contextFactory.CreateDbContext();
            employee.Id = CreateGUID();
            var employeeEntity = _mapper.Map<EmployeeEntity>(employee);
            context.Employees.Add(employeeEntity);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployee(string employeeId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var employeeEntity = new EmployeeEntity
                {
                    Id = employeeId
                };
                context.Employees.Remove(employeeEntity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new StorageErrorException($"Employee entity with Id {employeeId} was not found", 404);
            }
        }

        public async Task<Employee> GetEmployee(string employeeId)
        {
            using var context = _contextFactory.CreateDbContext();
            var employeeEntity = await context.Employees.FindAsync(employeeId);
            if (employeeEntity == null)
            {
                throw new StorageErrorException($"Employee entity with Id {employeeId} was not found", 404);
            }
            var employee = _mapper.Map<Employee>(employeeEntity);
            return employee;
        }

        public async Task<List<Employee>> GetEmployees(string municipalityId)
        {
            using var context = _contextFactory.CreateDbContext();
            var employeeEntities = await context.Employees
                .Where(employeeEntity => employeeEntity.MunicipalityId == municipalityId)
                .ToListAsync();
            var employees = _mapper.Map<List<Employee>>(employeeEntities);
            return employees;

        }

        public async Task UpdateEmployee(Employee employee)
        {
            using var context = _contextFactory.CreateDbContext();
            var employeeEntity = await context.Employees.FindAsync(employee.Id);
            if (employeeEntity == null)
            {
                throw new StorageErrorException($"Employee entity with Id {employee.Id} was not found", 404);
            }
            _mapper.Map(employee, employeeEntity);
            await context.SaveChangesAsync();
        }

        private string CreateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
