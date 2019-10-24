using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Scripts.Observer;
using Observer;
using UnityEngine;

public class MyTrackableEventHandler : DefaultTrackableEventHandler
{
    public GameObject map;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        //map.SetActive(true);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        //map.SetActive(false);
    }
}