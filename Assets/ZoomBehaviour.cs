using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 zoomedScale;
    [SerializeField] private double secondsZoomed;
    
    private Vector3 _defaultScale;
    private bool _isDoubleTapping;
    private bool _isZoomed;
    private float _timer;

    private void Start()
    {
        _defaultScale = transform.localScale;
        
        if (zoomedScale.Equals(Vector3.zero))
        {
            zoomedScale = _defaultScale * 1.5f;
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        CheckDoubleTap();

        if (_isDoubleTapping && !_isZoomed)
        {
            ActivateZoom();
            _isZoomed = true;
        }
        else if (_isZoomed)
        {
            if (_timer <= secondsZoomed) return;

            DeactivateZoom();
            ResetTimer();
            _isZoomed = false;
        }
    }

    private void CheckDoubleTap()
    {
        _isDoubleTapping = false;

        foreach (var touch in Input.touches)
        {
            if (touch.tapCount != 2) continue;
            
            _isDoubleTapping = true;
            Debug.Log("ZOOM: double tap detected");
        }
    }

    private void ActivateZoom()
    {
        Debug.Log("ZOOM: activating");
        transform.localScale = zoomedScale;
    }

    private void DeactivateZoom()
    {
        Debug.Log("ZOOM: deactivating");
        transform.localScale = _defaultScale;
    }

    private void ResetTimer()
    {
        _timer = 0;
    }
}