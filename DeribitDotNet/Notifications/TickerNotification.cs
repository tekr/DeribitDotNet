using System;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Notifications
{
    public class TickerNotification : Ticker, INotification
    {
        [JsonIgnore]
        public DateTime ArrivalTime { get; set; }

        public void Notify(DeribitApi api) => api.TickersSubject.OnNext(this);
    }
}