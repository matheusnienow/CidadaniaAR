using UnityEngine;

public class MyTrackableEventHandler : DefaultTrackableEventHandler
{

    public GameObject player;
    
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        player.SetActive(true);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        player.SetActive(false);
    }
}