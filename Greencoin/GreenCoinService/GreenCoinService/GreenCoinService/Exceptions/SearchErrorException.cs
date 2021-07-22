using System;

namespace GreenCoinService.Exceptions
{
    public class SearchErrorException : Exception
    {
        public SearchErrorException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public SearchErrorException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
