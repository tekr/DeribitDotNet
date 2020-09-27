
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeribitDotNet.Notifications
{
    public class NotificationHolder
    {
        [JsonProperty("params")]
        public NotificationDetails Details;
    }

    public class NotificationDetails
    {
        public string Channel;
        public JContainer Data;
    }
}
