using Assets.Scripts.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour, IObservable<EventPlayerDestinationReached>
{
    public NavMeshAgent agent;
    public GameObject Destination { get; set; }

    private List<IObserver<EventPlayerDestinationReached>> observers;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDestination();
    }

    public void Move()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        Debug.Log("PlayerMovementController: MOVING THE PLAYER TO DESTINATION: " + Destination.name);
        var destinationPosition = Destination.transform.position;
        agent.SetDestination(destinationPosition);
    }

    void CheckDestination()
    {
        if (Destination == null)
        {
            return;
        }

        var currentPosition = transform.position;
        var destinationPosition = Destination.transform.position;

        var distance = Mathf.Abs(Vector3.Distance(currentPosition, destinationPosition));
        Debug.Log("PlayerMovementController: Player to Destination distance: " + (distance < 0.5));

        if (distance < 0.5)
        {
            OnDestinationReached();
        }
    }

    private void OnDestinationReached()
    {
        Debug.Log("PlayerMovementController: Destination rechead");
        agent.isStopped = true;
        agent.ResetPath();
        Destination = null;
        NotifyDestinationReached();
    }

    private void NotifyDestinationReached()
    {
        Debug.Log("PlayerMovementController: NOTIFYING DESTINATION REACHED");
        if (observers == null || observers.Count == 0)
        {
            Debug.Log("PlayerMovementController: OBSERVERS NULL");
            return;
        }

        observers.ForEach(o =>
        {
            o.OnNext(new EventPlayerDestinationReached(Destination));
            o.OnCompleted();
        });
    }

    public IDisposable Subscribe(IObserver<EventPlayerDestinationReached> observer)
    {
        if (observers == null)
        {
            observers = new List<IObserver<EventPlayerDestinationReached>>();
        }

        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }

        return new Unsubscriber(observers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<EventPlayerDestinationReached>> _observers;
        private IObserver<EventPlayerDestinationReached> _observer;

        public Unsubscriber(List<IObserver<EventPlayerDestinationReached>> observers, IObserver<EventPlayerDestinationReached> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (!(_observer == null)) _observers.Remove(_observer);
        }
    }
}
