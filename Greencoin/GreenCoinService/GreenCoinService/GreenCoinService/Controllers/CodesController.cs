using GreenCoinService.DataContracts;
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
    public class CodesController : ControllerBase
    {
        private readonly ICodesService _codesService;

        public CodesController(ICodesService codesService)
        {
            _codesService = codesService;
        }

        /// <summary>
        /// Fetch waste code of a user. Can be used by the users of by the municipality admins for printing.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("waste/{userId}")]
        [ProducesResponseType(typeof(GetWasteCodesResponse), 201)]
        public async Task<IActionResult> GetWasteCodes(string userId)
        {
            var wasteCodes = await _codesService.GetWasteCodes(userId);
            var getWasteCodesResponse = new GetWasteCodesResponse
            {
                Codes = wasteCodes
            };
            return Ok(getWasteCodesResponse);
        }

        /// <summary>
        /// Fetch business code. Can be used by businesses or municipality admins for printing.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("business/{businessId}")]
        [ProducesResponseType(typeof(Code), 201)]
        public async Task<IActionResult> GetBusinessCode(string businessId)
        {
            var businessCode = await _codesService.GetBusinessCode(businessId);
            return Ok(businessCode);
        }

        /// <summary>
        /// Fetch information from code. Intended to be used by operators and employees.
        /// If the code is a waste code, it will return userId and recyclableId. 
        /// If the code is a business code, it will return businessId.
        /// </summary>
        /// <param name="codeId">The code extracted from the QR Code.</param>
        /// <returns></returns>
        [HttpGet("{codeId}")]
        [ProducesResponseType(typeof(Code), 201)]
        public async Task<IActionResult> GetCode(string codeId)
        {
            var code = await _codesService.GetCode(codeId);
            return Ok(code);
        }
    }
}
