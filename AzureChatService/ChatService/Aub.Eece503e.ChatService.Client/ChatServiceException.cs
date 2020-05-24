using System;
using System.Net;


namespace Aub.Eece503e.ChatService.Client
{
    public class ChatServiceException : Exception
    {
        public ChatServiceException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
  