using System;
using System.Collections.Generic;
using Assets.Scripts.Observer;
using Observer;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovementController : MonoBehaviour, IObservable<EventPlayerDestinationReached>
{
    private NavMeshAgent _agent;
    private NavMeshAgent Agent
    {
        get
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            return _agent;
        }
        
        set => _agent = value;
    }

    public GameObject Destination { private get; set; }

    private List<IObserver<EventPlayerDestinationReached>> _observers;

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
        var destinationPosition = Destination.transform.position;
        SetAgentDestination(destinationPosition);
    }

    private void SetAgentDestination(Vector3 destinationPosition)
    {
        var navMeshSurface = GameObject.Find("NavMeshSurface");
        navMeshSurface.SetActive(true);
        
        var isOnNavMesh = Agent.isOnNavMesh;

        if (isOnNavMesh)
        {
            Agent.SetDestination(destinationPosition);
            Debug.Log("PlayerMovementController: MOVING THE PLAYER TO DESTINATION: " + Destination.name);
        }
        else
        {
            Debug.Log("PlayerMovementController: Agent is not active or is not in a NavMesh");
        }
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
        //Debug.Log("PlayerMovementController: Player to Destination distance: " + (distance < 0.5));

        if (distance < 0.5f)
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
        if (_observers == null || _observers.Count == 0)
        {
            Debug.Log("PlayerMovementController: OBSERVERS NULL");
            return;
        }

        _observers.ForEach(o =>
        {
            o.OnNext(new EventPlayerDestinationReached(Destination));
            o.OnCompleted();
        });
    }

    public IDisposable Subscribe(IObserver<EventPlayerDestinationReached> observer)
    {
        if (_observers == null)
        {
            _observers = new List<IObserver<EventPlayerDestinationReached>>();
        }

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubscriber<EventPlayerDestinationReached>(_observers, observer);
    }

    public void Warp(Vector3 position)
    {
        Agent.Warp(position);
    }
}
