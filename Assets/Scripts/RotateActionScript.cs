using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActionScript : MonoBehaviour, ActionScript {

    private float rotationTarget;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Execute()
    {
        Debug.Log("Calling Activate()");
        rotationTarget = transform.rotation.y == 90 ? 180 : 90;
        transform.Rotate(0, rotationTarget, 0, Space.Self);
    }
}
