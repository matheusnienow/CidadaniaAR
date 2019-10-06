using System;
using System.Collections.Generic;
using Assets.Scripts.Observer;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour, IObservable<EventPlayerDestinationReached>
{
    private NavMeshAgent agent;
    private NavMeshAgent Agent
    {
        get
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            return agent;
        }
        
        set => agent = value;
    }

    public GameObject Destination { private get; set; }

    private List<IObserver<EventPlayerDestinationReached>> observers;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckDestination();
    }

    public void Move()
    {

        Debug.Log("PlayerMovementController: MOVING THE PLAYER TO DESTINATION: " + Destination.name);
        var destinationPosition = Destination.transform.position;
        Agent.SetDestination(destinationPosition);
    }

    private void CheckDestination()
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
        Debug.Log("PlayerMovementController: Destination reached");
        Agent.isStopped = true;
        Agent.ResetPath();
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
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null) _observers.Remove(_observer);
        }
    }
}
