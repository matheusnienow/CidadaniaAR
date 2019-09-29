using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Observer
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
