using System;
using System.Collections.Generic;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Puzzles.Base
{
    public abstract class Puzzle : MonoBehaviour, IObservable<Message>
    {
        private List<IObserver<Message>> observers;

        protected void Start()
        {
            this.observers = new List<IObserver<Message>>();
            
        }

        public IDisposable Subscribe(IObserver<Message> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);

        }

        public void NotifyOnNext()
        {
            if (observers == null)
            {
                return;
            }

            var onNextMessage = GetOnNextMessage();
            observers.ForEach(o =>
            {
                o.OnNext(onNextMessage);
            });
        }

        public void NotifyOnCompleted()
        {
            observers?.ForEach(o =>
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
