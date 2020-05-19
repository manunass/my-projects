using Aub.Eece503e.ChatService.Datacontracts;
using Aub.Eece503e.ChatService.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Aub.Eece503e.ChatService.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("{conversationId}/messages")]
        public async Task<IActionResult> PostMessage([FromBody] Message message, string conversationId)
        {
            await _conversationService.AddMessage(message, conversationId);
            return CreatedAtAction(nameof(GetMessages), new { conversationId = conversationId }, message);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(string conversationId, string continuationToken, int limit, long lastSeenMessageTime)
        {
            GetMessagesResult result = await _conversationService.GetMessages(conversationId, continuationToken, limit, lastSeenMessageTime);

            string nextUri = null;
            if (!string.IsNullOrWhiteSpace(result.ContinuationToken))
            {
                nextUri = $"api/conversations/{conversationId}/messages?continuationToken={WebUtility.UrlEncode(result.ContinuationToken)}&limit={limit}&lastseenMessageTime={lastSeenMessageTime}";
            }

            List<GetMessagesResponseItem> messages = new List<GetMessagesResponseItem>();
            foreach (var message in result.Messages)
            {
                messages.Add(
                new GetMessagesResponseItem
                {
                    Text = message.Text,
                    SenderUsername = message.SenderUsername,
                    UnixTime = message.UnixTime
                });
            }

            var response = new GetMessagesResponse
            {
                NextUri = nextUri,
                Messages = messages
            };
            return Ok(response);
        }

        [HttpDelete("{conversationId}/messages/{messageId}")]
        public async Task <IActionResult> DeleteMessage(string conversationId, string messageId)
        {
            await _conversationService.DeleteMessage(conversationId, messageId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostConversation([FromBody]AddConversationRequestBody body)
        {
            var firstMessage = body.FirstMessage;
            var participants = body.Participants;
            AddConversationResult result = await _conversationService.AddConversation(firstMessage, participants);
            var response = new AddConversationResponse
            {
                Id = result.Id,
                CreatedUnixTime = result.CreatedUnixTime
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations(string username, string continuationToken, int limit, long lastSeenConversationTime)
        {
            GetConversationsResult result = await _conversationService.GetConversations(username, continuationToken, limit, lastSeenConversationTime);

            string nextUri = null;
            if (!string.IsNullOrWhiteSpace(result.ContinuationToken))
            {
                nextUri = $"api/conversations?username={username}&continuationToken={WebUtility.UrlEncode(result.ContinuationToken)}&limit={limit}&lastseenConversationTime={lastSeenConversationTime}";
            }

            var response = new GetConversationsResponse
            {
                NextUri = nextUri,
                Conversations = result.GetConversationsItems
            };
            return Ok(response);
        }

        [HttpDelete("{conversationId}")]
        public async Task<IActionResult> DeleteConversation(string conversationId)
        {
            await _conversationService.DeleteConversation(conversationId);
            return Ok();
        }

    }
}
