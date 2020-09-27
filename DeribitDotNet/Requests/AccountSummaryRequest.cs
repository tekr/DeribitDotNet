using DeribitDotNet.Responses;

namespace DeribitDotNet.Requests
{
    public class AccountSummaryRequest : Request<AccountSummaryResponse>
    {
        public DeribitCurrency Currency;

        public bool Extended;

        public AccountSummaryRequest(DeribitCurrency currency) : base("get_account_summary", false)
        {
            Currency = currency;
            Extended = true;
        }
    }
}