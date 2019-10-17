using System;
using System.Collections.Generic;
using Assets.Scripts.Observer;
using Observer;
using UnityEngine;

namespace Puzzles.Base
{
    public abstract class Puzzle : MonoBehaviour, IObservable<Message>
    {
        private List<IObserver<Message>> _observers;

        protected void Start()
        {
            this._observers = new List<IObserver<Message>>();
            
        }

        public IDisposable Subscribe(IObserver<Message> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new Unsubscriber<Message>(_observers, observer);

        }

        public void NotifyOnNext()
        {
            if (_observers == null)
            {
                return;
            }

            var onNextMessage = GetOnNextMessage();
            _observers.ForEach(o =>
            {
                o.OnNext(onNextMessage);
            });
        }

        public void NotifyOnCompleted()
        {
            _observers?.ForEach(o =>
            {
                o.OnCompleted();
            });
        }

        protected abstract Message GetOnNextMessage();

        protected abstract bool IsConditionMet();

        protected abstract void OnConditionMet();

        protected abstract void OnConditionNotMet();
    }
}
