using System;
using DeribitDotNet.Requests;
using DeribitDotNet.Responses;
using Xunit;

namespace DeribitDotNet.UnitTests
{
    public class SerialisationTests
    {
        [Fact]
        public void DeserialiseResponse()
        {
            var response1 = SerialisationHelper.Deserialise<TradesResponse>(@"{""jsonrpc"":""2.0"",""id"":3,""usIn"":1561290404443695,""usOut"":1561290404449496,""usDiff"":5801,""testnet"":true}");

            Assert.Equal(3, response1.Id);
            Assert.Equal(5801, response1.ProcessingTimeUs);
            Assert.Equal(DateTime.FromBinary(636968872044436950), response1.ExchangeTimeIn);

            var response2 = SerialisationHelper.Deserialise<OpenOrdersResponse>(@"{""jsonrpc"":""2.0"",""id"":4,""result"":[],""usIn"":1561290352204918,""usOut"":1561290352205398,""usDiff"":480,""testnet"":true}");

            Assert.Equal(4, response2.Id);
            Assert.Equal(480, response2.ProcessingTimeUs);
            Assert.Equal(DateTime.FromBinary(636968871522049180), response2.ExchangeTimeIn);
        }
    }
}
