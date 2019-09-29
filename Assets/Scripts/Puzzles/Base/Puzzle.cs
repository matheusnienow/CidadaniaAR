using Assets.Scripts.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Puzzles
{
    public abstract class Puzzle : MonoBehaviour, IObservable<Message>
    {
        private List<IObserver<Message>> observers;

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
            if (observers == null)
            {
                return;
            }

            observers.ForEach(o =>
            {
                o.OnCompleted();
            });
        }

        internal abstract Message GetOnNextMessage();

        protected abstract bool IsConditionMet();

        protected abstract void OnConditionMet();

        protected abstract void OnConditionNotMet();
    }
}
