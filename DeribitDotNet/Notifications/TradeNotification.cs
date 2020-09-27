using System;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Notifications
{
    public class TradeNotification : Trade, INotification
    {
        [JsonIgnore]
        public DateTime ArrivalTime { get; set; }

        public void Notify(DeribitApi api) => api.TradesSubject.OnNext(this);
    }
}