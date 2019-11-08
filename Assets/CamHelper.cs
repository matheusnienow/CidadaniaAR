using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using Util;

public class CamHelper : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject target;
    [SerializeField] private DirectionEnum targetDirection;

    private Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var deltaZ = PuzzleTools.GetDirectionDifference(gameObject, target, targetDirection);

        var blockCenter = target.GetComponent<Renderer>().bounds.center;
        var camPosition = transform.position;
        
        var deltaX = PuzzleTools.GetXDifference(camPosition, blockCenter);
        var deltaY = PuzzleTools.GetYDifference(camPosition, blockCenter);
        
        var desiredLocation = new Vector3(deltaX, deltaY, deltaZ);
        arrow.transform.LookAt(desiredLocation);
    }
}