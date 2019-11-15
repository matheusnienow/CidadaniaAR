using System;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace Puzzles.Base
{
    public abstract class Puzzle : MonoBehaviour, IObservable<EventPuzzle>
    {
        private List<IObserver<EventPuzzle>> _observers;

        protected void Start()
        {
            if (_observers != null)
            {
                return;
            }

            _observers = new List<IObserver<EventPuzzle>>();
        }

        public IDisposable Subscribe(IObserver<EventPuzzle> observer)
        {
            if (_observers == null)
            {
                _observers = new List<IObserver<EventPuzzle>>();
            }

            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<EventPuzzle>(_observers, observer);
        }

        protected void NotifyOnNext(EventPuzzle eventPuzzle)
        {
            _observers?.ForEach(o => { o.OnNext(eventPuzzle); });
        }

        public void NotifyOnCompleted()
        {
            _observers?.ForEach(o => { o.OnCompleted(); });
        }

        protected abstract bool IsConditionMet();

        protected abstract void OnConditionMet();

        protected abstract void OnConditionNotMet();
    }
}