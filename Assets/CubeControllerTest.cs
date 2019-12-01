using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CubeControllerTest : MonoBehaviour
{
    [SerializeField] private GameObject cube2;

    void Start()
    {
    }

    void Update()
    {
        const double directionThreshold = 0.3;
        var forward = transform.forward;
        var cube2Forward = cube2.transform.forward;

        var dotResult = Vector3.Dot(forward.normalized, cube2Forward.normalized);
        Debug.DrawRay(transform.position, forward * 5, Color.magenta);
        Debug.DrawRay(cube2.transform.position, cube2Forward * 5, Color.magenta);

        var maxValue = (1 + directionThreshold);
        var minValue = (1 - directionThreshold);

        var result = dotResult < maxValue && dotResult > minValue;
        
        Debug.Log("Dot product: " + dotResult + " Result: "+result);
    }
}