using System;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Notifications
{
    public class UserTradeNotification : UserTrade, INotification
    {
        [JsonIgnore]
        public DateTime ArrivalTime { get; set; }

        public void Notify(DeribitApi api) => api.UserTradesSubject.OnNext(this);
    }
}