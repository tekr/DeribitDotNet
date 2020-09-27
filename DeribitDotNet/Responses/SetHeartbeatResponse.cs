using System.Collections.Generic;

namespace DeribitDotNet.Responses
{
    public class SetHeartbeatResponse : Response
    {
        public string Result;

        public override string ToString() => $"{base.ToString()}, {nameof(Result)}: {Result}";
    }
}