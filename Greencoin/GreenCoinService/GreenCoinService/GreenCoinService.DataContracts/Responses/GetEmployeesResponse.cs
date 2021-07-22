using System;
using System.Collections.Generic;
using System.Text;

namespace GreenCoinService.DataContracts.Responses
{
    public class GetEmployeesResponse
    {
        public List<Employee> Employees { get; set; }
    }
}
