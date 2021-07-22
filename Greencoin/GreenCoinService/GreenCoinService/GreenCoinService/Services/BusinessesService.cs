using GreenCoinService.DataContracts;
using GreenCoinService.Services.Interfaces;
using GreenCoinService.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenCoinService.Services
{
    public class BusinessesService : IBusinessesService
    {
        private readonly IBusinessesStore _businessesStore;
        private readonly IImagesService _imagesService;
        private readonly ICodesService _codesService;

        public BusinessesService(IBusinessesStore businessesStore, IImagesService imagesService, ICodesService codesService)
        {
            _businessesStore = businessesStore;
            _imagesService = imagesService;
            _codesService = codesService;
        }

        public async Task<Business> AddBusiness(Business business)
        {
            InitializeBusiness(business);
            var addBusinessResult = await _businessesStore.AddBusiness(business);
            await _codesService.CreateBusinessCode(business.Id);
            return addBusinessResult;
        }

        public async Task DeleteBusiness(string businessId)
        {
            await _businessesStore.DeleteBusiness(businessId);
            await _imagesService.DeleteImagesFolder(businessId);
        }

        public async Task<Business> GetBusiness(string businessId)
        {
            var business = await _businessesStore.GetBusiness(businessId);
            var imagesUris = await _imagesService.ListImagesUris(businessId);
            business.ImagesUris = imagesUris;
            return business;
        }

        public async Task<List<Business>> GetBusinesses(string municipalityId)
        {
            var businesses = await _businessesStore.GetBusinesses(municipalityId);
            foreach(var business in businesses)
            {
                var imagesUris = await _imagesService.ListImagesUris(business.Id);
                business.ImagesUris = imagesUris;
            }
            return businesses;
        }

        public async Task<List<Business>> SearchBusinesses(string municipalityId, string category, string name)
        {
            var businesses = await _businessesStore.SearchBusinesses(municipalityId, category, name);
            foreach (var business in businesses)
            {
                var imagesUris = await _imagesService.ListImagesUris(business.Id);
                business.ImagesUris = imagesUris;
            }
            return businesses;
        }

        public async Task UpdateBusiness(Business business)
        {
            await _businessesStore.UpdateBusiness(business);
        }

        private static void InitializeBusiness(Business business)
        {
            business.Wallet = new Wallet
            {
                Balance = 0,
                Score = 0
            };
        }
    }
}
