using UnityEngine;

namespace Observer
{
    public class EventPlayerDestinationReached
    {
        public GameObject Destination { get; set; }

        public EventPlayerDestinationReached(GameObject destination)
        {
            Destination = destination;
        }
    }
}
