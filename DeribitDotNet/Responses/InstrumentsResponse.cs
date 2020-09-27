using System;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Responses
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum InstrumentType
    {
        Future,
        Option
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum SettlementInterval
    {
        Perpetual,
        Month,
        Week,
        Day
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum OptionType
    {
        Call,
        Put
    }

    public class InstrumentsResponse : Response
    {
        public class Instrument
        {
            [JsonProperty("base_currency")]
            public string BaseCurrency;

            [JsonProperty("contract_size")]
            public double ContractSize;

            [JsonProperty("creation_timestamp")]
            [JsonConverter(typeof(CountToDateTimeConverter), false)]
            public DateTime Created;

            [JsonProperty("expiration_timestamp")]
            [JsonConverter(typeof(CountToDateTimeConverter), false)]
            public DateTime Expiration;

            [JsonProperty("instrument_name")]
            public string InstrumentName;

            [JsonProperty("is_active")]
            public bool IsActive;

            [JsonProperty("kind")]
            public InstrumentType InstrumentType;

            [JsonProperty("min_trade_amount")]
            public double MinTradeAmount;

            [JsonProperty("option_type")]
            public OptionType? OptionType;

            [JsonProperty("quote_currency")]
            public string Currency;

            [JsonProperty("settlement_period")]
            public SettlementInterval Settlement;

            [JsonProperty("tick_size")]
            public double TickSize;

            // *** Only for options
            public double? Strike;

            public override string ToString()
            {
                return $"{nameof(BaseCurrency)}: {BaseCurrency}, {nameof(ContractSize)}: {ContractSize}, {nameof(Created)}: " +
                       $"{Created}, {nameof(Expiration)}: {Expiration}, {nameof(InstrumentName)}: {InstrumentName}, {nameof(IsActive)}: " +
                       $"{IsActive}, {nameof(InstrumentType)}: {InstrumentType}, {nameof(MinTradeAmount)}: {MinTradeAmount}, " +
                       $"{nameof(OptionType)}: {OptionType}, {nameof(Currency)}: {Currency}, {nameof(Settlement)}: {Settlement}, " +
                       $"{nameof(TickSize)}: {TickSize}, {nameof(Strike)}: {Strike}";
            }
        }

        [JsonProperty("result")] public Instrument[] Instruments;

        public override string ToString() =>
            $"{base.ToString()}, {nameof(Instruments)}: [{string.Join("],[", (object[]) Instruments)}]";
    }
}