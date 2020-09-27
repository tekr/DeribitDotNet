using System;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;

namespace DeribitDotNet.Notifications
{
    public class OrderBookNotification : INotification
    {
        [JsonIgnore]
        public DateTime ArrivalTime { get; set; }

        public LevelEvent[] Bids;
        public LevelEvent[] Asks;

        [JsonProperty("change_id")]
        public long ChangeId;

        [JsonProperty("prev_change_id")]
        public long PreviousChangeId;

        [JsonProperty("instrument_name")]
        public string Instrument;

        public long Timestamp;
        
        public string Type;

        public void Notify(DeribitApi api) => api.OrderBooksSubject.OnNext(this);

        public override string ToString() =>
            $"Instrument: {Instrument} Type: {Type} ChangeId: {ChangeId} PrevChangeId: {PreviousChangeId}\n" +
            $"Bids: {string.Join(", ", Bids)}\n" + $"Asks: {string.Join(", ", Asks)}";
    }

    public enum EventType
    {
        New,
        Change,
        Delete
    }

    [JsonConverter(typeof(ObjectToArrayConverter<LevelEvent>))]
    public struct LevelEvent
    {
        [JsonProperty(Order = 1)]
        public EventType Type;

        [JsonProperty(Order = 2)]
        public double Price;

        [JsonProperty(Order = 3)]
        public int Amount;

        public override string ToString() => $"{Type} {Amount}@{Price}";
    }
}