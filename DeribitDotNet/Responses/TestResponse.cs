using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class TestResponse : Response
    {
        public IDictionary<string, string> Result;

        public override string ToString() => $"{base.ToString()}, {nameof(Result)}: {Result}";
    }
}