using System;
using System.Collections.Generic;

namespace Assets.Scripts.Observer
{
    public class Unsubscriber : IDisposable
    {
        private List<IObserver<Message>> _observers;
        private IObserver<Message> _observer;

        public Unsubscriber(List<IObserver<Message>> observers, IObserver<Message> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (!(_observer == null)) _observers.Remove(_observer);
        }
    }
}
