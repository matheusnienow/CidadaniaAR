using UnityEngine;
using Vuforia;

namespace Util
{
    public static class VuforiaTools
    {
        public static bool IsBeingTracked(string imageTargetName)
        {
            var imageTarget = GameObject.Find(imageTargetName);
            if (!imageTarget)
            {
                return false;
            }

            var trackable = imageTarget.GetComponent<TrackableBehaviour>();
            var status = trackable.CurrentStatus;

            return status == TrackableBehaviour.Status.TRACKED;
        }
    }
}