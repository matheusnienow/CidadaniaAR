using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActionScript : MonoBehaviour {

    private float rotationTarget;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        Debug.Log("Calling Activate()");
        rotationTarget = transform.rotation.y == 90 ? 180 : 90;
        transform.Rotate(0, rotationTarget, 0, Space.Self);
    }
}
