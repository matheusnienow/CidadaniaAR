using Enum;
using UnityEngine;

namespace Observer
{
    public class EventPuzzle
    {
        public EPuzzleStatus Status { get; set; }

        public bool IsTimed { get; set; }

        public string GameObjectName { get; set; }

        public EventPuzzle(EPuzzleStatus status, bool isTimed, string brokenObjectName)
        {
            Status = status;
            IsTimed = isTimed;
            GameObjectName = brokenObjectName;
        }

        public EventPuzzle(EPuzzleStatus status)
        {
            Status = status;
            IsTimed = false;
            GameObjectName = null;
        }
    }
}