using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DeribitDotNet.Notifications;
using DeribitDotNet.Responses;

namespace DeribitDotNet.Demo
{
    public class OrderBook
    {
        private readonly SortedDictionary<double, Level> _bids = new SortedDictionary<double, Level>(new ReverseComparer<double>());
        private readonly SortedDictionary<double, Level> _asks = new SortedDictionary<double, Level>();

        private readonly Subject<Quote> _subject = new Subject<Quote>();

        private readonly int _levels;

        public IObservable<Quote> Quotes => _subject.DistinctUntilChanged().AsObservable();

        public OrderBook(int levels) => _levels = levels;

        public void Update(OrderBookNotification notification)
        {
            if (notification.PreviousChangeId == 0)
            {
                _bids.Clear();
                _asks.Clear();
            }

            ProcessItems(notification.Bids, Direction.Buy);
            ProcessItems(notification.Asks, Direction.Sell);

            _subject.OnNext(GetQuote(this, notification.ArrivalTime));
        }

        private void ProcessItems(LevelEvent[] levelEvents, Direction direction)
        {
            var levels = direction == Direction.Buy ? _bids : _asks;

            foreach (var levelEvent in levelEvents)
            {
                switch (levelEvent.Type)
                {
                    case EventType.New:
                    case EventType.Change:
                        levels[levelEvent.Price] = new Level(levelEvent.Price, levelEvent.Amount);
                        break;

                    case EventType.Delete:
                        levels.Remove(levelEvent.Price);
                        break;
                }
            }
        }

        private Quote GetQuote(OrderBook ob, DateTime time) => new Quote(time, GetList(ob._bids.Values), GetList(ob._asks.Values));

        private IList<Level> GetList(IEnumerable<Level> levels) => levels.Take(_levels).ToList();

        private sealed class ReverseComparer<T> : IComparer<T>
        {
            int IComparer<T>.Compare(T x, T y) => ((IComparer<T>)Comparer<T>.Default).Compare(y, x);
        }
    }
}
