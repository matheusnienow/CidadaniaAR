using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Scripts.Observer;
using LevelManager;
using Observer;
using UnityEngine;

public class MyTrackableEventHandler : DefaultTrackableEventHandler, IObservable<EventTargetTracking>
{
    private List<IObserver<EventTargetTracking>> _observers;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        NotifyTracking(true);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        NotifyTracking(false);
    }

    private void NotifyTracking(bool found)
    {
        _observers?.ForEach(o => { o.OnNext(new EventTargetTracking(found)); });
    }

    public IDisposable Subscribe(IObserver<EventTargetTracking> observer)
    {
        if (_observers == null)
        {
            _observers = new List<IObserver<EventTargetTracking>>();
        }

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubscriber<EventTargetTracking>(_observers, observer);
    }
}