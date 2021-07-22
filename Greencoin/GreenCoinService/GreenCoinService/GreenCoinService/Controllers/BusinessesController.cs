using AutoMapper;
using GreenCoinService.DataContracts;
using GreenCoinService.DataContracts.Requests;
using GreenCoinService.DataContracts.Responses;
using GreenCoinService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenCoinService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessesService _businessesService;
        private readonly IMapper _mapper;
        public BusinessesController(IBusinessesService businessesService)
        {
            _businessesService = businessesService;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateBusinessRequest, Business>();
                cfg.CreateMap<Business, BusinessInfo>();
                cfg.CreateMap<Business, SearchBusinessesReponse>();
            });
            _mapper = configuration.CreateMapper();
        }
        /// <summary>
        /// Create a business. Can be used by users to register their business to the app.
        /// This will create a wallet and an indentification code linked to the business.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Business), 201)]
        public async Task<IActionResult> Add([FromBody] CreateBusinessRequest createBusinessRequest)
        {
            var business = _mapper.Map<Business>(createBusinessRequest);
            var addBusinessResult = await _businessesService.AddBusiness(business);
            return CreatedAtAction(nameof(Get), new { businessId = addBusinessResult.Id }, addBusinessResult);
        }

        /// <summary>
        /// Fetch a business. Contains confidential information. Can be used by the business or by the municipality admin.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}")]
        [ProducesResponseType(typeof(Business), 200)]
        public async Task<IActionResult> Get(string businessId)
        {
            var business = await _businessesService.GetBusiness(businessId);
            return Ok(business);
        }

        /// <summary>
        /// Fetch business info. Can be used by anyone.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}/Info")]
        [ProducesResponseType(typeof(BusinessInfo), 200)]
        public async Task<IActionResult> GetInfo(string businessId)
        {
            var business = await _businessesService.GetBusiness(businessId);
            var businessInfo = _mapper.Map<BusinessInfo>(business);
            return Ok(businessInfo);
        }

        /// <summary>
        /// Search Businesses. Can be used by users or by municipality employees.
        /// </summary>
        /// <param name="municipalityId">The ID of the municipality in which the business is registered.</param>
        /// <param name="category">The category of the business.</param>
        /// <param name="name">The name of the business.</param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(SearchBusinessesReponse), 200)]
        public async Task<IActionResult> Search(string municipalityId, string category = null, string name = null)
        {
            var businesses = await _businessesService.SearchBusinesses(municipalityId, category, name);
            var businessesInfo = _mapper.Map<List<BusinessInfo>>(businesses);
            var searchBusinessesResponse = new SearchBusinessesReponse
            {
                Businesses = businessesInfo
            };
            return Ok(searchBusinessesResponse);
        }

        /// <summary>
        /// Update a business. Can be used by the business.
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="updateBusinessRequest"></param>
        /// <returns></returns>
        [HttpPut("{businessId}")]
        public async Task<IActionResult> Update(string businessId, [FromBody] UpdateBusinessRequest updateBusinessRequest)
        {
            var business = await _businessesService.GetBusiness(businessId);
            UpdateBusinessProperties(business, updateBusinessRequest);
            await _businessesService.UpdateBusiness(business);
            return Ok();
        }

        /// <summary>
        /// Delete a business. Can be used by businesses to delete their account.
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpDelete("{businessId}")]
        public async Task<IActionResult> Delete(string businessId)
        {
            await _businessesService.DeleteBusiness(businessId);
            return Ok();
        }

        private static void UpdateBusinessProperties(Business business, UpdateBusinessRequest updateBusinessRequest)
        {
            business.Name = updateBusinessRequest.Name;
            business.About = updateBusinessRequest.About;
            business.Address = updateBusinessRequest.Address;
            business.Category = updateBusinessRequest.Category;
            business.OwnerFirstName = updateBusinessRequest.OwnerFirstName;
            business.OwnerLastName = updateBusinessRequest.OwnerLastName;
        }
    }
}
