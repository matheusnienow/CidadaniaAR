using Enum;
using UnityEngine;

namespace Observer
{
    public class EventPuzzle
    {
        public PuzzleStatus Status { get; set; }

        public bool IsTimed { get; set; }

        public string GameObjectName { get; set; }

        public EventPuzzle(PuzzleStatus status, bool isTimed, string brokenObjectName)
        {
            Status = status;
            IsTimed = isTimed;
            GameObjectName = brokenObjectName;
        }

        public EventPuzzle(PuzzleStatus status)
        {
            Status = status;
            IsTimed = false;
            GameObjectName = null;
        }
    }
}