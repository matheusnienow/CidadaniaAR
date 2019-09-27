using Assets.Scripts.Observer;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;

    public delegate void OnDestinationReached();
    public static event OnDestinationReached onDestinationReached;

    private void Start()
    {
        destination = GameObject.FindWithTag("Destination").transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var playerPosition = Mathf.Round(transform.position.z);
        var destinationPosition = Mathf.Round(destination.z);

        //Debug.Log("player: " + playerPosition + ", destination: " + destinationPosition);
        if (playerPosition != destinationPosition)
        {
            //Debug.Log("setting destination");
            agent.SetDestination(destination);
        }
        else
        {
            onDestinationReached?.Invoke();
        }
    }
}
