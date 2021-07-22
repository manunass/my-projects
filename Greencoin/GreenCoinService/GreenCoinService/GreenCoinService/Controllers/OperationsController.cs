using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationsService _operationsService;
        private readonly IMunicipalitiesService _municipalitiesService;
        private readonly IMapper _mapper;

        public OperationsController(IOperationsService operationsService, IMunicipalitiesService municipalitiesService)
        {
            _operationsService = operationsService;
            _municipalitiesService = municipalitiesService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateRecyclableRequest, Recyclable>();
                cfg.CreateMap<CreateBagScanRequest, BagScan>();
            });
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// Create a recyclable. Intended to be added once before all municipalities by the app admins. 
        /// If a new recyclable is created, a new batch (#0) will be created for all municipalities, and new waste codes are created for all users.
        /// </summary>
        /// <param name="createRecyclableRequest"></param>
        /// <returns></returns>
        [HttpPost("recyclables")]
        [ProducesResponseType(typeof(Recyclable), 201)]
        public async Task<IActionResult> CreateRecyclable([FromBody] CreateRecyclableRequest createRecyclableRequest)
        {
            var recyclable = _mapper.Map<Recyclable>(createRecyclableRequest);
            var municipalities = await _municipalitiesService.GetMunicipalities();
            var createRecyclableResult = await _operationsService.AddRecyclable(recyclable, municipalities);
            return CreatedAtAction(nameof(GetRecyclable), new { recyclableId = createRecyclableResult.Id }, createRecyclableResult);
        }

        /// <summary>
        /// Fetch all recyclables. Intended to be used by all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("recyclables")]
        [ProducesResponseType(typeof(GetRecyclablesResponse), 201)]
        public async Task<IActionResult> GetRecyclables()
        {
            var recyclables = await _operationsService.GetRecyclables();
            var getRecyclablesResponse = new GetRecyclablesResponse
            {
                Recyclables = recyclables
            };
            return Ok(getRecyclablesResponse);
        }

        /// <summary>
        /// Fetch a recyclable. Intended to be used by all users.
        /// </summary>
        /// <param name="recyclableId"></param>
        /// <returns></returns>
        [HttpGet("recyclables/{recyclableId}")]
        [ProducesResponseType(typeof(Recyclable), 201)]
        public async Task<IActionResult> GetRecyclable(string recyclableId)
        {
            var recyclable = await _operationsService.GetRecyclable(recyclableId);
            return Ok(recyclable);
        }

        /// <summary>
        /// Update the information of a recyclable. Can be used by app admins.
        /// </summary>
        /// <param name="updateRecyclableRequest"></param>
        /// <param name="recyclableId"></param>
        /// <returns></returns>
        [HttpPut("recyclables/{recyclableId}")]
        public async Task<IActionResult> UpdateRecyclable([FromBody] UpdateRecyclableRequest updateRecyclableRequest, string recyclableId)
        {
            var recyclable = await _operationsService.GetRecyclable(recyclableId);
            UpdateRecyclableProperties(recyclable, updateRecyclableRequest);
            await _operationsService.UpdateRecyclable(recyclable);
            return Ok();
        }

        /// <summary>
        /// Delete a recyclable. Can be used by app admins. 
        /// This will not delete related batches and codes
        /// </summary>
        /// <param name="recyclableId"></param>
        /// <returns></returns>
        [HttpDelete("recyclables/{recyclableId}")]
        public async Task<IActionResult> DeleteRecyclable(string recyclableId)
        {
            await _operationsService.DeleteRecyclable(recyclableId);
            return Ok();
        }

        /// <summary>
        /// Fetch current batch for a recyclable. Can be used by municipality admins.
        /// </summary>
        /// <param name="municipalityId"></param>
        /// <param name="recyclableId"></param>
        /// <returns></returns>
        [HttpGet("{municipalityId}/recyclables/{recyclableId}/batches")]
        [ProducesResponseType(typeof(Batch), 201)]
        public async Task<IActionResult> GetCurrentBatch(string municipalityId, string recyclableId)
        {
            var batch = await _operationsService.GetCurrentBatch(municipalityId, recyclableId);
            return Ok(batch);
        }

        /// <summary>
        /// Process a batch. This will set all bag scans within a batch to processed, and credit the wallets of users with the corresponding number of coins.
        /// Coins per bag = (LbpCoinRation x Batch Revene / Total Weight of All Bags) x Bag Weight 
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpPatch("recyclables/batches/{batchId}")]
        public async Task<IActionResult> ProcessBatch(string batchId)
        {
            var batch = await _operationsService.GetBatch(batchId);
            var municipality = await _municipalitiesService.GetMunicipality(batch.MunicipalityId);
            await _operationsService.ProcessBatch(municipality, batch);
            return Ok();
        }

        /// <summary>
        /// Update batch information. Can be used by municipality admins.
        /// Intended to be used to update Batch Revenue.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="updateBatchRequest"></param>
        /// <returns></returns>
        [HttpPut("recyclables/batches/{batchId}")]
        public async Task<IActionResult> UpdateBatch(string batchId, [FromBody] UpdateBatchRequest updateBatchRequest)
        {
            var batch = await _operationsService.GetBatch(batchId);
            UpdateBatchProperties(batch, updateBatchRequest);
            await _operationsService.UpdateBatch(batch);
            return Ok();
        }

        /// <summary>
        /// Create a bag scan. Can be used by municipality employees.
        /// Note: No need to input the Batch Id, the backend will automatically add the Bag Scan to the current Batch for a certain recyclable.
        /// </summary>
        /// <param name="createBagScanRequest"></param>
        /// <param name="municipalityId"></param>
        /// <param name="recyclableId"></param>
        /// <returns></returns>
        [HttpPost("{municipalityId}/recyclables/{recyclableId}/batches/bag-scans")]
        [ProducesResponseType(typeof(BagScan), 201)]
        public async Task<IActionResult> CreateBagScan([FromBody] CreateBagScanRequest createBagScanRequest, string municipalityId, string recyclableId)
        {
            var bagScan = _mapper.Map<BagScan>(createBagScanRequest);
            var createBagScanResult = await _operationsService.AddBagScan(bagScan, municipalityId, recyclableId);
            return Ok(createBagScanResult);
        }

        /// <summary>
        /// Fetch all Bag Scans of a Batch. Can be used by municipality admins.
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet("recyclables/batches/{batchId}/bag-scans")]
        [ProducesResponseType(typeof(GetBagScansResponse), 201)]
        public async Task<IActionResult> GetBagScans(string batchId)
        {
            var bagScans = await _operationsService.GetBagScans(batchId);
            var getBagScansResponse = new GetBagScansResponse
            {
                BagScans = bagScans
            };
            return Ok(getBagScansResponse);
        }

        /// <summary>
        /// Search Bag Scans.
        /// Intended to be used by all platfrom users with search parameters that correspond to their level of authorization:
        /// (i.e. users can only search by userId, municipality admins by all parameters, and operators by employeeId)
        /// Will throw an error if userId, employeeId, and batchId are all null.
        /// </summary>
        /// <param name="userId">The ID of the user who the bag belongs to.</param>
        /// <param name="employeeId">The ID of the employee who scanned the bag.</param>
        /// <param name="batchId">The ID of the batch that the bag scan belongs to.</param>
        /// <param name="processed">Whether the bag scan is processed or not.</param>
        /// <param name="unixTimeScannedLow">Determines a lower limit to the time the bags were scanned. No lower limit if unspecified.</param>
        /// <param name="unixTimeScannedHigh">Determines an upper limit to the time the bags were scanned. No upper limit if unspecified.</param>
        /// <returns></returns>
        [HttpGet("recyclables/batches/bag-scans")]
        [ProducesResponseType(typeof(GetBagScansResponse), 201)]
        public async Task<IActionResult> GetBagScans(string userId = null, string employeeId = null, string batchId = null, bool? processed = null, long? unixTimeScannedLow = null, long? unixTimeScannedHigh = null)
        {
            CheckSearchBagScansParameters(userId, employeeId, batchId);
            var bagScans = await _operationsService.SearchBagScans(userId, employeeId, batchId, processed, unixTimeScannedLow, unixTimeScannedHigh);
            var searchBagScansResponse = new SearchBagScansResponse
            {
                BagScans = bagScans
            };
            return Ok(searchBagScansResponse);
        }
        /// <summary>
        /// Fetch a Bag Scan. Can be used by users (only the owners), or by the municipality admins.
        /// </summary>
        /// <param name="bagScanId"></param>
        /// <returns></returns>
        [HttpGet("recyclables/batches/bag-scans/{bagScanId}")]
        [ProducesResponseType(typeof(BagScan), 201)]
        public async Task<IActionResult> GetBagScan(string bagScanId)
        {
            var bagScan = await _operationsService.GetBagScan(bagScanId);
            return Ok(bagScan);
        }

        private static void UpdateRecyclableProperties(Recyclable recyclable, UpdateRecyclableRequest updateRecyclableRequest)
        {
            recyclable.LbpPerKg = updateRecyclableRequest.LbpPerKg;
        }

        private static void UpdateBatchProperties(Batch batch, UpdateBatchRequest updateBatchRequest)
        {
            batch.Revenue = updateBatchRequest.Revenue;
        }

        private static void CheckSearchBagScansParameters(string userId, string employeeId, string batchId)
        {
            if(userId == null && employeeId == null && batchId == null)
            {
                throw new ArgumentNullException("At least one of userId, employeeId, or batchId has to be non-null.");
            }
        }
    }
}
