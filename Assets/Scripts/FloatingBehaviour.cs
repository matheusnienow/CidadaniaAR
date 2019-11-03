using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float frequency = 1f;

    // Position Storage Variables
    private Vector3 _posOffset;
    private Vector3 _tempPos;
    
    private void Start()
    {
        // Store the starting position & rotation of the object
        _posOffset = transform.position;
    }
    
    private void Update()
    {
        var currentPosition = transform.position;
        _tempPos = new Vector3(currentPosition.x, _posOffset.y, currentPosition.z);
        _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = _tempPos;
    }
}