using HumanityService.Client.DataContracts.Results;
using HumanityService.DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HumanityService.Client
{
    class HumanityServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly Uri baseAddress = new Uri("https://localhost:44324");

        public HumanityServiceClient()
        {
            var httpClient = new HttpClient { BaseAddress = baseAddress };
            _httpClient = httpClient;
        }

        public async Task<UserInfo> GetUserInfo(string username)
        {
            var responseMessage = await _httpClient.GetAsync($"api/users/users/{username}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedUser = JsonConvert.DeserializeObject<UserInfo>(json);
            return fetchedUser;
        }

        public async Task<NgoInfo> GetNgoInfo(string username)
        {
            var responseMessage = await _httpClient.GetAsync($"api/users/ngos/{username}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedUser = JsonConvert.DeserializeObject<NgoInfo>(json);
            return fetchedUser;
        }

        public async Task<AuthenticationResult> LoginUser(LoginRequest loginRequest)
        {
            string json = JsonConvert.SerializeObject(loginRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/login-user")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string recievedJson = await responseMessage.Content.ReadAsStringAsync();
            var authResult = JsonConvert.DeserializeObject<AuthenticationResult>(recievedJson);
            return authResult;
        }

        public async Task<AuthenticationResult> LoginNgo(LoginRequest loginRequest)
        {
            string json = JsonConvert.SerializeObject(loginRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/authentication/login-ngo")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string recievedJson = await responseMessage.Content.ReadAsStringAsync();
            var authResult = JsonConvert.DeserializeObject<AuthenticationResult>(recievedJson);
            return authResult;
        }


        public async Task<Campaign> GetCampaign(string campaignId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/transactions/campaigns/{campaignId}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedCampaign = JsonConvert.DeserializeObject<Campaign>(json);
            return fetchedCampaign;
        }

        public async Task<GetCampaignsResult> GetCampaigns(string ngoName = null, string username = null, string type = null, string category = null, string status = null)
        {
            var uri = BuildGetCampaignsUri(ngoName, username, type, category, status);
            var responseMessage = await _httpClient.GetAsync(uri);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var getCampaignsResult = JsonConvert.DeserializeObject<GetCampaignsResult>(json);
            return getCampaignsResult;
        }

        public async Task<Campaign> MatchCampaign(MatchCampaignRequest matchCampaignRequest)
        {
            string json = JsonConvert.SerializeObject(matchCampaignRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/campaigns/match")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string recievedJson = await responseMessage.Content.ReadAsStringAsync();
            var campaign = JsonConvert.DeserializeObject<Campaign>(recievedJson);
            return campaign;
        }

        public async Task CreateCampaign(CreateCampaignRequest createCampaignRequest)
        {
            string json = JsonConvert.SerializeObject(createCampaignRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/campaigns")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task AnswerCampaign(string campaignId, AnswerCampaignRequest answerCampaignRequest)
        {
            string json = JsonConvert.SerializeObject(answerCampaignRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/transactions/campaigns/answer-campaign/{campaignId}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task EditCampaign(string campaignId, EditCampaignRequest editCampaignRequest)
        {
            string json = JsonConvert.SerializeObject(editCampaignRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"api/transactions/campaigns/{campaignId}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task CancelCampaign(string campaignId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/transactions/campaings/{campaignId}");
            var responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task<DeliveryDemand> GetDeliveryDemand(string deliveryDemandId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/transactions/delivery-demands/{deliveryDemandId}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedDeliveryDemand = JsonConvert.DeserializeObject<DeliveryDemand>(json);
            return fetchedDeliveryDemand;
        }

        public async Task<GetDeliveryDemandsResult> GetDeliveryDemands(string processId = null, string campaignName = null, string pickupUsername = null, string destinationUsername = null, string status = null)
        {
            var uri = BuildGetDeliveryDemandsUri(processId, campaignName, pickupUsername, destinationUsername, status);
            var responseMessage = await _httpClient.GetAsync(uri);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var getDeliveryDemandsResult = JsonConvert.DeserializeObject<GetDeliveryDemandsResult>(json);
            return getDeliveryDemandsResult;
        }

        public async Task<MatchDeliveryDemandResult> MatchDeliveryDemand(MatchDeliveryDemandRequest matchDeliveryDemandRequest)
        {
            string json = JsonConvert.SerializeObject(matchDeliveryDemandRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/delivery-demands/match")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string recievedJson = await responseMessage.Content.ReadAsStringAsync();
            var matchDeliveryDemandResult = JsonConvert.DeserializeObject<MatchDeliveryDemandResult>(recievedJson);
            return matchDeliveryDemandResult;
        }

        public async Task AnswerDeliveryDemand(string deliveryDemandId, AnswerDeliveryDemandRequest answerDeliveryDemandRequest)
        {
            string json = JsonConvert.SerializeObject(answerDeliveryDemandRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/transactions/delivery-demands/answer-delivery-demand/{deliveryDemandId}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task<Contribution> GetContribution(string contributionId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/transactions/contributions/{contributionId}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedContribution = JsonConvert.DeserializeObject<Contribution>(json);
            return fetchedContribution;
        }

        public async Task<GetContributionsResult> GetContributions(string username = null, string processId = null, string deliveryDemandId = null, string type = null, string status = null)
        {
            var uri = BuildGetContributionsUri(username, processId, deliveryDemandId, type, status);
            var responseMessage = await _httpClient.GetAsync(uri);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var getContributionsResult = JsonConvert.DeserializeObject<GetContributionsResult>(json);
            return getContributionsResult;
        }

        public async Task ApproveContribution(string contributionId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/transactions/contributions/approve/{contributionId}");
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }
        public async Task ValidateContribution(string contributionId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/transactions/contributions/validate/{contributionId}");
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task CancelContribution(string contributionId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"api/transactions/contributions/{contributionId}");
            var responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
        }

        public async Task<Process> GetProcess(string processId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/transactions/processes/{processId}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedProcess = JsonConvert.DeserializeObject<Process>(json);
            return fetchedProcess;
        }

        public async Task<GetProcessesResult> GetProcesses(string campaignId)
        {
            var responseMessage = await _httpClient.GetAsync($"api/transactions/processes?campaignId={campaignId}");
            await EnsureSuccessOrThrowAsync(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var getProcessesResult = JsonConvert.DeserializeObject<GetProcessesResult>(json);
            return getProcessesResult;
        }

        public async Task<ValidateDeliveryResult> ValidateDelivery(ValidateDeliveryRequest validateDeliveryRequest)
        {
            string json = JsonConvert.SerializeObject(validateDeliveryRequest);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/transactions/validate-delivery")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(request);
            await EnsureSuccessOrThrowAsync(responseMessage);
            string recievedJson = await responseMessage.Content.ReadAsStringAsync();
            var validateDeliveryResult = JsonConvert.DeserializeObject<ValidateDeliveryResult>(recievedJson);
            return validateDeliveryResult;
        }

        private static async Task EnsureSuccessOrThrowAsync(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                string message = $"{await responseMessage.Content.ReadAsStringAsync()}";
                throw new HumanityServiceException(message, responseMessage.StatusCode);
            }
        }

        string BuildGetCampaignsUri(string ngoName = null, string username = null, string type = null, string category = null, string status = null)
        {
            List<string> parameters = new List<string>();
            AddParameter("ngoName", ngoName, parameters);
            AddParameter("username", username, parameters);
            AddParameter("type", type, parameters);
            AddParameter("category", category, parameters);
            AddParameter("status", status, parameters);

            string joinedParameters = string.Join("&", parameters);
            return $"api/transactions/campaigns?{joinedParameters}";
        }

        string BuildGetDeliveryDemandsUri(string processId = null, string campaignName = null, string pickupUsername = null, string destinationUsername = null, string status = null)
        {
            List<string> parameters = new List<string>();
            AddParameter("processId", processId, parameters);
            AddParameter("campaignName", campaignName, parameters);
            AddParameter("pickupUsername", pickupUsername, parameters);
            AddParameter("destinationUsername", destinationUsername, parameters);
            AddParameter("status", status, parameters);

            string joinedParameters = string.Join("&", parameters);
            return $"api/transactions/delivery-demands?{joinedParameters}";
        }

        string BuildGetContributionsUri(string username = null, string processId = null, string deliveryDemandId = null, string type = null, string status = null)
        {
            List<string> parameters = new List<string>();
            AddParameter("username", username, parameters);
            AddParameter("processId", processId, parameters);
            AddParameter("deliveryDemandId", deliveryDemandId, parameters);
            AddParameter("type", type, parameters);
            AddParameter("status", status, parameters);

            string joinedParameters = string.Join("&", parameters);
            return $"api/transactions/contributions?{joinedParameters}";
        }


        static void AddParameter(string parameterName, object parameterValue, List<string> parameters)
        {
            if (parameterValue == null) return;
            parameters.Add($"{parameterName}={parameterValue}");
        }
    }
}
