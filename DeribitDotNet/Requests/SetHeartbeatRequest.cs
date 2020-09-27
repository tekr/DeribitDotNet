using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class SetHeartbeatRequest : Request<SetHeartbeatResponse>
    {
        [JsonProperty("interval")]
        public int IntervalSec;

        public SetHeartbeatRequest(int intervalSec) : base("set_heartbeat", true)
        {
            IntervalSec = intervalSec;
        }
    }
}