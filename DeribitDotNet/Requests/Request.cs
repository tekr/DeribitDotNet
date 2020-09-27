using DeribitDotNet.Responses;

namespace DeribitDotNet.Requests
{
    public abstract class Request
    {
        internal bool IsPublic;
        internal string Method;

        public override string ToString() => $"{nameof(IsPublic)}: {IsPublic}, {nameof(Method)}: {Method}";

        protected static InstrumentKind? AnyAsNull(InstrumentKind instrumentKind) =>
            instrumentKind != InstrumentKind.Any ? instrumentKind : (InstrumentKind?)null;

        protected static OrderType? AnyAsNull(OrderType orderType) => orderType != OrderType.Any ? orderType : (OrderType?)null;
    }

    public abstract class Request<TResponse> : Request where TResponse : Response
    {
        protected Request(string method, bool isPublic)
        {
            Method = method;
            IsPublic = isPublic;
        }
    }
}