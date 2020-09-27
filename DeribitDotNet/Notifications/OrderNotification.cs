using System;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Notifications
{
    public class OrderNotification : Order, INotification
    {
        [JsonIgnore]
        public DateTime ArrivalTime { get; set; }

        public void Notify(DeribitApi api) => api.OrdersSubject.OnNext(this);
    }
}