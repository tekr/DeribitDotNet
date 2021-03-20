using System;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Responses
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum TickerState
    {
        Open,
        Closed,
    }

    public class Ticker
    {
        [JsonProperty("best_bid_amount")]
        public int BestBidQty;

        [JsonProperty("best_bid_price", NullValueHandling = NullValueHandling.Ignore)]
        public double BestBidPrice;

        [JsonProperty("best_ask_amount")]
        public int BestAskQty;

        [JsonProperty("best_ask_price", NullValueHandling = NullValueHandling.Ignore)]
        public double BestAskPrice;

        [JsonProperty("current_funding")]
        public double? FundingRate;

        [JsonProperty("delivery_price")]
        public double? DeliveryPrice;

        [JsonProperty("funding_8h")]
        public double? FundingRate8HAvg;

        [JsonProperty("index_price")]
        public double IndexPrice;

        [JsonProperty("instrument_name")]
        public string Instrument;

        [JsonProperty("last_price")]
        public double LastPrice;

        [JsonProperty("mark_price")]
        public double MarkPrice;

        [JsonProperty("max_price")]
        public double MaxBuyPrice;

        [JsonProperty("min_price")]
        public double MinSellPrice;

        [JsonProperty("open_interest")]
        public long OpenInterest;

        [JsonProperty("settlement_price")]
        public double? SettlementPrice;

        public TickerState State;

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CountToDateTimeConverter), false)]
        public DateTime DateTime;

        public override string ToString() =>
            $"{nameof(BestBidQty)}: {BestBidQty}, {nameof(BestBidPrice)}: {BestBidPrice}, {nameof(BestAskQty)}: {BestAskQty}, " +
            $"{nameof(BestAskPrice)}: {BestAskPrice}, {nameof(FundingRate)}: {FundingRate}, {nameof(DeliveryPrice)}: " +
            $"{DeliveryPrice}, {nameof(FundingRate8HAvg)}: {FundingRate8HAvg}, {nameof(IndexPrice)}: {IndexPrice}, " +
            $"{nameof(Instrument)}: {Instrument}, {nameof(LastPrice)}: {LastPrice}, {nameof(MarkPrice)}: {MarkPrice}, " +
            $"{nameof(MaxBuyPrice)}: {MaxBuyPrice}, {nameof(MinSellPrice)}: {MinSellPrice}, {nameof(OpenInterest)}: {OpenInterest}, " +
            $"{nameof(SettlementPrice)}: {SettlementPrice}, {nameof(State)}: {State}, {nameof(DateTime)}: {DateTime}";
    }
}