using Aub.Eece503e.ChatService.Datacontracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Client
{
    public class ChatServiceClient : IChatServiceClient
    {
        private readonly HttpClient _httpClient;

        public ChatServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddUser(User user)
        {
            string json = JsonConvert.SerializeObject(user);
            HttpResponseMessage responseMessage = await _httpClient.PostAsync("api/profile", new StringContent(json, Encoding.UTF8,
                "application/json"));
            EnsureSuccessOrThrow(responseMessage);
        }

        public async Task<User> GetUser(string username)
        {
            var responseMessage = await _httpClient.GetAsync($"api/profile/{username}");
            EnsureSuccessOrThrow(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            var fetchedStudent = JsonConvert.DeserializeObject<User>(json);
            return fetchedStudent;
        }

        public async Task UpdateUser(User user)
        {
            var body = new UpdateUserRequestBody
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureId=user.ProfilePictureId
            };

            string json = JsonConvert.SerializeObject(body);
            HttpResponseMessage responseMessage = await _httpClient.PutAsync($"api/profile/{user.Username}", new StringContent(json,
                Encoding.UTF8, "application/json"));
            EnsureSuccessOrThrow(responseMessage);
        }

        public async Task DeleteUser(string username)
        {
            var responseMessage = await _httpClient.DeleteAsync($"api/profile/{username}");
            EnsureSuccessOrThrow(responseMessage);
        }

        public async Task<UploadImageResponse> UploadImage(Stream stream)
        {
            HttpContent fileStreamContent = new StreamContent(stream);
            fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "NotNeeded"
            };
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"api/images")
                {
                    Content = formData
                };

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                EnsureSuccessOrThrow(response);
                var json = await response.Content.ReadAsStringAsync();
                var imageResponse = JsonConvert.DeserializeObject<UploadImageResponse>(json);
                return imageResponse;
            }
        }
       
        public async Task<DownloadImageResponse> DownloadImage(string imageId)
        {
            using (HttpResponseMessage response = await _httpClient.GetAsync($"api/images/{imageId}"))
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                EnsureSuccessOrThrow(response); 
                return new DownloadImageResponse(bytes);
            }
        }

        public async Task DeleteImage(string imageId) 
        {
            var responseMessage = await _httpClient.DeleteAsync($"api/images/{imageId}");
            EnsureSuccessOrThrow(responseMessage);
        }

        public async Task AddMessage(string conversationId, Message message)
        {
            string json = JsonConvert.SerializeObject(message);
            HttpResponseMessage responseMessage = await _httpClient.PostAsync($"api/conversations/{conversationId}/messages", new StringContent(json, Encoding.UTF8, "application/json"));
            EnsureSuccessOrThrow(responseMessage);
        }

        public  Task <GetMessagesResponse> GetMessages (string conversationId, int limit, long lastSeenMessageTime)
        {
            return GetMessagesByUri($"api/conversations/{conversationId}/messages?limit={limit}&lastSeenMessageTime={lastSeenMessageTime}");
        }

        public async Task<GetMessagesResponse> GetMessagesByUri(string uri)
        {
            var responseMessage = await _httpClient.GetAsync(uri);
            EnsureSuccessOrThrow(responseMessage);
            string json = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetMessagesResponse>(json);
        }

        public async Task DeleteMessage(string conversationId, string messageId)
        {
            var responseMessage = await _httpClient.DeleteAsync($"api/conversations/{conversationId}/messages/{messageId}");
            EnsureSuccessOrThrow(responseMessage);
        }

        public async Task<AddConversationResponse> AddConversation(Message firstMessage, List <string> participants)
        {
            var body = new AddConversationRequestBody
            {
                Participants = participants,
                FirstMessage = firstMessage
            };
            string json = JsonConvert.SerializeObject(body);
            var responseMessage = await _httpClient.PostAsync($"api/conversations", new StringContent(json, Encoding.UTF8, "application/json"));
            EnsureSuccessOrThrow(responseMessage);
            string returnJson = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AddConversationResponse>(returnJson);

        }

        public Task<GetConversationsResponse> GetConversations(string username, int limit, long lastSeenConversationTime)
        {
            return GetConversationsByUri($"api/conversations?username={username}&limit={limit}&lastSeenConversationTime={lastSeenConversationTime}");
        }

        public async Task<GetConversationsResponse> GetConversationsByUri(string uri)
        {
            var response= await _httpClient.GetAsync(uri);
            EnsureSuccessOrThrow(response);
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetConversationsResponse>(json);
        }

        public async Task DeleteConversation(string conversationId)
        {
            var responseMessage = await _httpClient.DeleteAsync($"api/conversations/{conversationId}");
            EnsureSuccessOrThrow(responseMessage);
        }

        private static void EnsureSuccessOrThrow(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new ChatServiceException("", responseMessage.StatusCode);
            }
        }
    }
}
