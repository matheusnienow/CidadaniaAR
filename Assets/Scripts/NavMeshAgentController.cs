using System;
using System.Collections.Generic;
using Assets.Scripts.Observer;
using Observer;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour, IObservable<EventPlayerDestinationReached>
{
   
    public GameObject Destination { private get; set; }

    public float distanceThreshold;

    private List<IObserver<EventPlayerDestinationReached>> _observers;

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
    
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updatePosition = true;
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
            Debug.Log("NavMeshAgentController: MOVING THE PLAYER TO DESTINATION: " + Destination.name);
        }
        else
        {
            Debug.Log("NavMeshAgentController: Agent is not active or is not in a NavMesh");
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
        //Debug.Log("NavMeshAgentController: Player to Destination distance: " + distance);

        if (distance < distanceThreshold)
        {
            OnDestinationReached();
        }
    }

    private void OnDestinationReached()
    {
        Debug.Log("NavMeshAgentController: Destination reached");
        Agent.isStopped = true;
        Agent.ResetPath();
        Destination = null;
        NotifyDestinationReached();
    }

    private void NotifyDestinationReached()
    {
        Debug.Log("NavMeshAgentController: NOTIFYING DESTINATION REACHED");
        if (_observers == null || _observers.Count == 0)
        {
            Debug.Log("NavMeshAgentController: OBSERVERS NULL");
            return;
        }

        _observers.ForEach(o =>
        {
            o.OnNext(new EventPlayerDestinationReached(Destination));
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
}
