using DeribitDotNet.Requests;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class AccountSummaryResponse : Response
    {
        [JsonProperty("result")]
        public AccountSummary AccountSummary;
    }

    public class AccountSummary
    {
        [JsonProperty("options_gamma")]
        public double OptionsGamma;

        [JsonProperty("projected_maintenance_margin")]
        public double ProjectedMaintenanceMargin;

        [JsonProperty("system_name")]
        public string SystemName;

        [JsonProperty("margin_balance")]
        public double MarginBalance;

        [JsonProperty("tfa_enabled")]
        public bool TfaEnabled;

        public string Username;

        public double Equity;

        [JsonProperty("futures_pl")]
        public double FuturesPnl;

        [JsonProperty("options_session_upl")]
        public double OptionsUnrealisedPnl;

        [JsonProperty("id")]
        public int AccountId;

        [JsonProperty("options_vega")]
        public double OptionsVega;

        [JsonProperty("session_funding")]
        public double SessionFunding;

        public DeribitCurrency Currency;

        [JsonProperty("type")]
        public string AccountType;

        [JsonProperty("futures_session_rpl")]
        public double FuturesRealisedPnl;

        [JsonProperty("options_theta")]
        public double OptionsTheta;

        [JsonProperty("portfolio_margining_enabled")]
        public bool PortfolioMarginEnabled;

        [JsonProperty("session_rpl")]
        public double SessionRealisedPnl;

        [JsonProperty("delta_total")]
        public double DeltaTotal;

        [JsonProperty("options_pl")]
        public double OptionsPnl;

        [JsonProperty("available_withdrawal_funds")]
        public double FundsAvailableToWithdraw;

        [JsonProperty("maintenance_margin")]
        public double MaintenanceMargin;

        [JsonProperty("initial_margin")]
        public double InitialMargin;

        [JsonProperty("futures_session_upl")]
        public double FuturesUnrealisedPnl;

        [JsonProperty("options_session_rpl")]
        public double OptionsRealisedPnl;

        [JsonProperty("available_funds")]
        public double FundsAvailable;

        public string Email;

        [JsonProperty("session_upl")]
        public double SessionUnrealisedPnl;

        [JsonProperty("total_pl")]
        public double TotalPnl;

        [JsonProperty("options_delta")]
        public double OptionsDelta;

        public double Balance;

        [JsonProperty("projected_initial_margin")]
        public double ProjectedInitialMargin;

        [JsonProperty("deposit_address")]
        public string DepositAddress;

        public override string ToString() =>
            $"{nameof(OptionsGamma)}: {OptionsGamma}, {nameof(ProjectedMaintenanceMargin)}: {ProjectedMaintenanceMargin}, " +
            $"{nameof(SystemName)}: {SystemName}, {nameof(MarginBalance)}: {MarginBalance}, {nameof(TfaEnabled)}: {TfaEnabled}, " +
            $"{nameof(Username)}: {Username}, {nameof(Equity)}: {Equity}, {nameof(FuturesPnl)}: {FuturesPnl}, " +
            $"{nameof(OptionsUnrealisedPnl)}: {OptionsUnrealisedPnl}, {nameof(AccountId)}: {AccountId}, {nameof(OptionsVega)}: " +
            $"{OptionsVega}, {nameof(SessionFunding)}: {SessionFunding}, {nameof(Currency)}: {Currency}, {nameof(AccountType)}: " +
            $"{AccountType}, {nameof(FuturesRealisedPnl)}: {FuturesRealisedPnl}, {nameof(OptionsTheta)}: {OptionsTheta}, " +
            $"{nameof(PortfolioMarginEnabled)}: {PortfolioMarginEnabled}, {nameof(SessionRealisedPnl)}: {SessionRealisedPnl}, " +
            $"{nameof(DeltaTotal)}: {DeltaTotal}, {nameof(OptionsPnl)}: {OptionsPnl}, {nameof(FundsAvailableToWithdraw)}: " +
            $"{FundsAvailableToWithdraw}, {nameof(MaintenanceMargin)}: {MaintenanceMargin}, {nameof(InitialMargin)}: {InitialMargin}, " +
            $"{nameof(FuturesUnrealisedPnl)}: {FuturesUnrealisedPnl}, {nameof(OptionsRealisedPnl)}: {OptionsRealisedPnl}, " +
            $"{nameof(FundsAvailable)}: {FundsAvailable}, {nameof(Email)}: {Email}, {nameof(SessionUnrealisedPnl)}: " +
            $"{SessionUnrealisedPnl}, {nameof(TotalPnl)}: {TotalPnl}, {nameof(OptionsDelta)}: {OptionsDelta}, {nameof(Balance)}: " +
            $"{Balance}, {nameof(ProjectedInitialMargin)}: {ProjectedInitialMargin}, {nameof(DepositAddress)}: {DepositAddress}";
    }
}