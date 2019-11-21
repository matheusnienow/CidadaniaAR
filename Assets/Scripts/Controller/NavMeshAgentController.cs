using System;
using System.Collections.Generic;
using Observer;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Util;

namespace Controller
{
    public class NavMeshAgentController : MonoBehaviour, IObservable<EventPlayerDestinationReached>
    {
        public GameObject Destination { private get; set; }

        [SerializeField] private float distanceThreshold;

        private List<IObserver<EventPlayerDestinationReached>> _observers;
        private NavMeshAgent _agent;
        private Text _text;

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
            _text = GameObject.Find("DistanceText")?.GetComponent<Text>();
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
            var destinationPosition = PuzzleTools.GetGameObjectBase(Destination);
            SetAgentDestination(destinationPosition);
        }

        private void SetAgentDestination(Vector3 destinationPosition)
        {
            var navMeshSurface = GameObject.Find("NavMeshSurface");
            navMeshSurface.SetActive(true);

            var isOnNavMesh = Agent.isOnNavMesh;
            if (!isOnNavMesh) return;

            Agent.SetDestination(destinationPosition);
            Agent.isStopped = false;
            Debug.Log("NavMeshAgentController: MOVING THE PLAYER TO DESTINATION: " + Destination.name + "(" +
                      destinationPosition + ")");
        }

        private void CheckDestination()
        {
            if (Destination == null)
            {
                if (_text != null) _text.text = "Destination null";
                return;
            }

            var currentPosition = transform.position;
            var destinationPosition = PuzzleTools.GetGameObjectBase(Destination);

            var distance = Mathf.Abs(Vector3.Distance(currentPosition, destinationPosition));
            //Debug.Log("NavMeshAgentController: Player to Destination distance: " + distance);
            if (_text != null) _text.text = distance.ToString();

            if (distance < distanceThreshold)
            {
                OnDestinationReached();
            }
        }

        private void OnDestinationReached()
        {
            Debug.Log("NavMeshAgentController: Destination reached");
            Stop();
            NotifyDestinationReached();
        }

        private void NotifyDestinationReached()
        {
            var destinationToNotify = Destination;
            Destination = null;
            Debug.Log("NavMeshAgentController: NOTIFYING DESTINATION REACHED");
            if (_observers == null || _observers.Count == 0)
            {
                Debug.Log("NavMeshAgentController: OBSERVERS NULL");
                return;
            }

            _observers.ForEach(o => { o.OnNext(new EventPlayerDestinationReached(destinationToNotify)); });
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

        public void Stop()
        {
            Agent.isStopped = true;
            Agent.ResetPath();
        }

        public void Teleport(Vector3 position)
        {
            Agent.isStopped = true;
            Agent.ResetPath();
            Agent.Warp(position);
        }
    }
}