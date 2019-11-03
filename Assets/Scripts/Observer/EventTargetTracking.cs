using UnityEngine;

namespace Observer
{
    public class EventTargetTracking
    {
        public bool IsVisible { get; }
        public EventTargetTracking(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
