using DeribitDotNet.Responses;

namespace DeribitDotNet.Requests
{
    public class TimeRequest : Request<TimeResponse>
    {
        public TimeRequest() : base("get_time", true)
        {
        }
    }
}