using System;

namespace DeribitDotNet.Notifications
{
    public interface INotification
    {
        DateTime ArrivalTime { get; set; }

        void Notify(DeribitApi api);
    }
}