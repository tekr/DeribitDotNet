using System;
using DeribitDotNet.Responses;

namespace DeribitDotNet
{
    public class DeribitException : Exception
    {
        public int Code => ErrorResponse.Code;

        public ErrorResponse ErrorResponse { get; }

        public DeribitException(ErrorResponse errorResponse): base(errorResponse.Message)
        {
            ErrorResponse = errorResponse;
        }

        public override string ToString() => $"{base.ToString()}, {nameof(ErrorResponse)}: {ErrorResponse}";
    }
}