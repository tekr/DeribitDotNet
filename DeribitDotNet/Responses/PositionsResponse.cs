using DeribitDotNet.JsonConverters;
using DeribitDotNet.Requests;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class Position
    {
        [JsonProperty("average_price")]
        public double AveragePrice;

        [JsonProperty("average_price_usd")]
        public double? AveragePriceUsd;

        public double Delta;

        [JsonConverter(typeof(PositionDirectionConverter))]
        public Direction Direction;

        [JsonProperty("estimated_liquidation_price")]
        public double EstimatedLiquidationPrice;

        [JsonProperty("floating_profit_loss")]
        public double UnrealisedPnl;

        [JsonProperty("floating_profit_loss_sd")]
        public double? UnrealisedPnlUsd;

        [JsonProperty("index_price")]
        public double IndexPrice;

        [JsonProperty("initial_margin")]
        public double InitialMargin;

        [JsonProperty("instrument_name")]
        public string Instrument;

        [JsonProperty("kind")]
        public InstrumentKind InstrumentKind;
        
        [JsonProperty("maintenance_margin")]
        public double MaintenanceMargin;

        [JsonProperty("mark_price")]
        public double MarkPrice;

        [JsonProperty("open_orders_margin")]
        public double OpenOrdersMargin;

        [JsonProperty("realized_profit_loss")]
        public double RealisedPnl;

        [JsonProperty("settlement_price")]
        public double SettlementPrice;

        public double Size;

        [JsonProperty("size_currency")]
        public double SizeInBaseCurrency;

        [JsonProperty("total_profit_loss")]
        public double TotalPnl;

        public override string ToString() => $"{nameof(AveragePrice)}: {AveragePrice}, {nameof(AveragePriceUsd)}: {AveragePriceUsd}, " +
                $"{nameof(Delta)}: {Delta}, {nameof(Direction)}: {Direction}, {nameof(EstimatedLiquidationPrice)}: " +
                $"{EstimatedLiquidationPrice}, {nameof(UnrealisedPnl)}: {UnrealisedPnl}, {nameof(UnrealisedPnlUsd)}: {UnrealisedPnlUsd}, " +
                $"{nameof(IndexPrice)}: {IndexPrice}, {nameof(InitialMargin)}: {InitialMargin}, {nameof(Instrument)}: {Instrument}, " +
                $"{nameof(InstrumentKind)}: {InstrumentKind}, {nameof(MaintenanceMargin)}: {MaintenanceMargin}, {nameof(MarkPrice)}: " +
                $"{MarkPrice}, {nameof(OpenOrdersMargin)}: {OpenOrdersMargin}, {nameof(RealisedPnl)}: {RealisedPnl}, " +
                $"{nameof(SettlementPrice)}: {SettlementPrice}, {nameof(Size)}: {Size}, {nameof(SizeInBaseCurrency)}: {SizeInBaseCurrency}, " +
                $"{nameof(TotalPnl)}: {TotalPnl}";
    }

    public class PositionsResponse : Response
    {
        [JsonProperty("result")]
        public Position[] Positions;
    }
}