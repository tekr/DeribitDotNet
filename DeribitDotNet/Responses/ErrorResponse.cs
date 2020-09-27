using Newtonsoft.Json.Linq;

namespace DeribitDotNet.Responses
{
    public class ErrorResponse
    {
        public int Code;

        public string Message;

        public JContainer Data;

        public override string ToString() => $"{nameof(Message)}: {Message}, {nameof(Code)}: {Code}";
    }
}