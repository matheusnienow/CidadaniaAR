using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class Subject
    {
        private List<IObserver> observers;

        public Subject()
        {
            observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer)
        {
            if (observer == null)
            {
                return;
            }

            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            if (observer == null)
            {
                return;
            }

            observers.Remove(observer);
        }

        protected void Notify(GameObject entity, Message message)
        {
            observers.ForEach(o => o.OnNotify(entity, message));
        }
    }
}
