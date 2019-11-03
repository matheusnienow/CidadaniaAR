using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBehaviour : MonoBehaviour
{
	[Range(0.1f, 1)]
	public float speed;
	
    private void Update()
    {
	    transform.Rotate(0, 1 * speed, 0, Space.Self);
    }
}
