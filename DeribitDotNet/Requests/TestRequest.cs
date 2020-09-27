using DeribitDotNet.Responses;

namespace DeribitDotNet.Requests
{
    public class TestRequest : Request<TestResponse>
    {
        public TestRequest() : base("test", true)
        {
        }
    }
}