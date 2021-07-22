using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.DataContracts.Requests;
using HumanityService.DataContracts.Results;
using HumanityService.Exceptions;
using HumanityService.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanityService.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IRoutingService _routingService;
        
        public MatchingService(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public async Task<Campaign> MatchUserToCampaign(List<Campaign> campaigns, MatchCampaignRequest request)
        {
            if (campaigns.Count == 0) return null;

            if (request.Type == "Donation")
            {
                List<Campaign> sortedCampaigns = campaigns.OrderBy(x => x.CompletedCount).ToList();
                return sortedCampaigns[0];
            }
            else if (request.Type == "Volunteering")
            {
                var campaignsETA = new List<(double, Campaign)>();
                foreach (var campaign in campaigns)
                {
                    var eta = await _routingService.GetETA(request.Location, campaign.Location, request.TransportationType);
                    campaignsETA.Add((eta, campaign));
                }
                //Sort the campaigns from closest to farthest and choose the closest
                campaignsETA.Sort();
                return campaignsETA[0].Item2;
            }
            else throw new BadRequestException("Invalid Request Type");
        }

        public async Task<MatchDeliveryDemandResult> MatchUserToDeliveryDemand(List<DeliveryDemand> deliveryDemandsRaw, MatchDeliveryDemandRequest request) 
        {

            var deliveryDemands = new List<DeliveryDemand>();
            // We filter out all active delivery demands that cannot fit in this time range (i.e mutually exclusive time windows)
            foreach (var deliveryDemand in deliveryDemandsRaw)
            {
                if(!(deliveryDemand.TimeWindowEnd < request.TimeWindowStart || request.TimeWindowEnd < deliveryDemand.TimeWindowStart))
                {
                    deliveryDemands.Add(deliveryDemand);
                }
            }

            //we get the estimated duration time that it would take for the deliverer to complete each delivery demand, 
            //and make sure that the delivery demand's eta is compatible with both the deliverer and the donor's time windows
            var deliveryDemandsETA = new List<(double, DeliveryDemand)>();
            foreach(var deliveryDemand in deliveryDemands)
            {
                var eta = await _routingService.GetETA(request.DelivererLocation, deliveryDemand.PickupLocation, deliveryDemand.DestinationLocation, request.TransportationType);
                if(request.TimeWindowStart + eta < request.TimeWindowEnd
                    && request.TimeWindowStart + eta > deliveryDemand.TimeWindowStart 
                    && request.TimeWindowStart + eta < deliveryDemand.TimeWindowEnd)
                {
                    deliveryDemandsETA.Add((eta, deliveryDemand));
                }
            }

            if (deliveryDemandsETA.Count == 0) return null;


            //Sort the delivery demands from least to most time consuming to deliverer
            deliveryDemandsETA.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            return new MatchDeliveryDemandResult
            {
                DeliveryDemand = deliveryDemandsETA[0].Item2,
                ETA = deliveryDemandsETA[0].Item1
            };
        }
    }
}
