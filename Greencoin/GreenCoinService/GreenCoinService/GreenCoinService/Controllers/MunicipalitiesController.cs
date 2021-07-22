using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalitiesController: ControllerBase
    {
        private readonly IMunicipalitiesService _municipalitiesService;
        private readonly IMapper _mapper;
        public MunicipalitiesController(IMunicipalitiesService municipalitiesService)
        {
            _municipalitiesService = municipalitiesService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Municipality, MunicipalityInfo>();
                cfg.CreateMap<CreateMunicipalityRequest, Municipality>();
                cfg.CreateMap<CreateEmployeeRequest, Employee>();
            });
            _mapper = configuration.CreateMapper();
        }
        
        /// <summary>
        /// Fetch a municipality. Can be used by municipality admins to access important information, and could contain confidential information.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <returns></returns>
        [HttpGet("{municipalityId}")]
        [ProducesResponseType(typeof(Municipality), 200)]
        public async Task<IActionResult> GetMunicipality(string municipalityId)
        {
            var municipality = await _municipalitiesService.GetMunicipality(municipalityId);
            return Ok(municipality);
        }

        /// <summary>
        /// Update the municipality object. Can be used to update variables such as LBPToCoinRatio and tresholds, or any other modifiable field.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <param name="updateMunicipalityRequest"></param>
        /// <returns></returns>
        [HttpPut("{municipalityId}")]
        public async Task<IActionResult> UpdateMunicipality(string municipalityId, [FromBody] UpdateMunicipalityRequest updateMunicipalityRequest)
        {
            var municipality = await _municipalitiesService.GetMunicipality(municipalityId);
            UpdateMunicipalityProperties(municipality, updateMunicipalityRequest);
            await _municipalitiesService.UpdateMunicipality(municipality);
            return Ok();
        }

        /// <summary>
        /// Fetch the information of a municipality. Can be used by users and businesses to access general information of a municipality (e.g. Phone Number or Location)
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <returns></returns>
        [HttpGet("{municipalityId}/info")]
        [ProducesResponseType(typeof(MunicipalityInfo), 200)]
        public async Task<IActionResult> GetMunicipalityInfo(string municipalityId)
        {
            var municipality = await _municipalitiesService.GetMunicipality(municipalityId);
            var municipalityInfo = _mapper.Map<MunicipalityInfo>(municipality);
            return Ok(municipalityInfo);
        }

        /// <summary>
        /// Fetch all municipalities. Contains all available information of any given municipality. Intended to be used by platform administrators and developers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetMunicipalitiesResponse), 200)]
        public async Task<IActionResult> GetMunicipalities()
        {
            var municipalities = await _municipalitiesService.GetMunicipalities();
            var getMunicipalitiesResponse = new GetMunicipalitiesResponse
            {
                Municipalities = municipalities
            };
            return Ok(getMunicipalitiesResponse);
        }

        /// <summary>
        /// Fecth a list of municipalities information. Can be used by users to pick desired municipality on sign-up.
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(GetMunicipalitiesInfoResponse), 200)]
        public async Task<IActionResult> GetMunicipalitiesInfo()
        {
            var municipalities = await _municipalitiesService.GetMunicipalities();
            var municipalitiesInfo = _mapper.Map<List<MunicipalityInfo>>(municipalities);
            var getMunicipalitiesInfoResponse = new GetMunicipalitiesInfoResponse
            {
                MunicipalitiesInfo = municipalitiesInfo
            };
            return Ok(getMunicipalitiesInfoResponse);
        }

        /// <summary>
        /// Create a municipality. Can be used by platform admins and developers.
        /// This will also create Batches (initialized at 0) for each recyclable for the municipality.
        /// </summary>
        /// <param name="createMunicipalityRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Municipality), 200)]
        public async Task<IActionResult> CreateMunicipality([FromBody] CreateMunicipalityRequest createMunicipalityRequest)
        {
            var municipality = _mapper.Map<Municipality>(createMunicipalityRequest);
            var addMunicipalityResult = await _municipalitiesService.AddMunicipality(municipality);
            return CreatedAtAction(nameof(GetMunicipality), new { municipalityId = addMunicipalityResult.Id }, addMunicipalityResult);
        }

        /// <summary>
        /// Delete a municipality. Can be used by platform admins and developers.
        /// Note: This will delete the municipality and all it's employees only. All other data will be preserved (Users, businesses, Batches, etc.)
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <returns></returns>
        [HttpDelete("{municipalityId}")]
        public async Task<IActionResult> DeleteMunicipality(string municipalityId)
        {
            await _municipalitiesService.DeleteMunicipality(municipalityId);
            return Ok();
        }

        /// <summary>
        /// Fetch employee information. Can be used by municipality admins on their own employees.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("/employees/{employeeId}")]
        [ProducesResponseType(typeof(Employee), 201)]
        public async Task<IActionResult> GetEmployee(string employeeId)
        {
            var employee = await _municipalitiesService.GetEmployee(employeeId);
            return Ok(employee);
        }

        /// <summary>
        /// Update employee information. Can be used by municipality admins on their own employees.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="updateEmployeeRequest"></param>
        /// <returns></returns>
        [HttpPut("/employees/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(string employeeId, [FromBody] UpdateEmployeeRequest updateEmployeeRequest)
        {
            var employee = await _municipalitiesService.GetEmployee(employeeId);
            UpdateEmployeeProperties(employee, updateEmployeeRequest);
            await _municipalitiesService.UpdateEmployee(employee);
            return Ok();
        }

        /// <summary>
        /// Fetch employees. Can be used by municipality admins on their own employees.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <returns></returns>
        [HttpGet("{municipalityId}/employees")]
        [ProducesResponseType(typeof(GetEmployeesResponse), 200)]
        public async Task<IActionResult> GetEmployees(string municipalityId)
        {
            var employees = await _municipalitiesService.GetEmployees(municipalityId);
            var getEmployeesResponse = new GetEmployeesResponse
            {
                Employees = employees
            };
            return Ok(getEmployeesResponse);
        }

        /// <summary>
        /// Create a new employee under a given municipality.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <param name="createEmployeeRequest"></param>
        /// <returns></returns>
        [HttpPost("{municipalityId}/employees")]
        [ProducesResponseType(typeof(Employee), 200)]
        public async Task<IActionResult> CreateEmployee(string municipalityId, [FromBody] CreateEmployeeRequest createEmployeeRequest)
        {
            var employee = _mapper.Map<Employee>(createEmployeeRequest);
            employee.MunicipalityId = municipalityId;
            var addEmployeeResult = await _municipalitiesService.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployee), new { municipalityId = municipalityId, employeeId = addEmployeeResult.Id }, addEmployeeResult);
        }

        /// <summary>
        /// Delete employee. Can be used by municipality admins on their own employees.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("{municipalityId}/employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            await _municipalitiesService.DeleteEmployee(employeeId);
            return Ok();
        }


        private static void UpdateMunicipalityProperties(Municipality municipality, UpdateMunicipalityRequest updateMunicipalityRequest)
        {
            municipality.Address = updateMunicipalityRequest.Address;
            municipality.CoinsInCirculation = updateMunicipalityRequest.CoinsInCirculation;
            municipality.CoinsCashoutTreshold = updateMunicipalityRequest.CoinsCashoutTreshold;
            municipality.PhoneNumber = updateMunicipalityRequest.PhoneNumber;
            municipality.LbpCoinRatio = updateMunicipalityRequest.LbpCoinRatio;
            municipality.Email = updateMunicipalityRequest.Email;
        }

        private static void UpdateEmployeeProperties(Employee employee, UpdateEmployeeRequest updateEmployeeRequest)
        {
            employee.FirstName = updateEmployeeRequest.FirstName;
            employee.LastName = updateEmployeeRequest.LastName;
            employee.PhoneNumber = updateEmployeeRequest.PhoneNumber;
            employee.Email = updateEmployeeRequest.Email;
            employee.Type = updateEmployeeRequest.Type;
        }
    }
}
