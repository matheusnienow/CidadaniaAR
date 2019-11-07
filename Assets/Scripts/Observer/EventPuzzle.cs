using Enum;

namespace Observer
{
    public class EventPuzzle
    {
        public EPuzzleStatus Status { get; set; }

        public bool IsTimed { get; set; }

        public EventPuzzle(EPuzzleStatus status, bool isTimed)
        {
            Status = status;
            IsTimed = isTimed;
        }

        public EventPuzzle(EPuzzleStatus status)
        {
            Status = status;
            IsTimed = false;
        }
    }

}
