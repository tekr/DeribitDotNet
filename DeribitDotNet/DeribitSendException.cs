using System;
using DeribitDotNet.Responses;

namespace DeribitDotNet
{
    public class DeribitSendException : Exception
    {
        public string ResponseMessage { get; }
        public ErrorResponse ErrorResponse { get; }

        public DeribitSendException(ErrorResponse errorResponse, string responseMessage, Exception innerException) : base(
            innerException.Message, innerException)
        {
            ErrorResponse = errorResponse;
            ResponseMessage = responseMessage;
        }

        public override string ToString() =>
            $"{base.ToString()}, {nameof(ResponseMessage)}: {ResponseMessage}, {nameof(ErrorResponse)}: {ErrorResponse}";
    }
}