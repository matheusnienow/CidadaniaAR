using System;
using Observer;
using UnityEngine;
using Util;

public class CamHelper : MonoBehaviour, IObserver<EventTargetTracking>
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject sign;
    [SerializeField] private MyTrackableEventHandler targetHandler;

    private GameObject _gameObject;
    private bool _isTargetVisible;
    private bool _isActive;

    private void Start()
    {
        arrow.SetActive(false);
        targetHandler.Subscribe(this);
        FocusOnBridge();
    }

    public void Init()
    {
        _isActive = true;
        arrow.SetActive(true);
    }

    private void Update()
    {
        if (!_isTargetVisible || !arrow.activeSelf || !_isActive) return;

        var blockCenter = PuzzleTools.GetPosition(_gameObject);
        var desiredLocation = new Vector3(blockCenter.x, blockCenter.y, blockCenter.z);
        arrow.transform.LookAt(desiredLocation);
    }

    private Vector3 GetPosition(GameObject g)
    {
        var blockCenter = PuzzleTools.GetPosition(_gameObject);
        var desiredLocation = new Vector3(blockCenter.x, blockCenter.y, blockCenter.z - 10000);

        return desiredLocation;
    }

    public void FocusOnBridge()
    {
        _gameObject = bridge;
    }

    public void FocusOnSign()
    {
        _gameObject = sign;
    }

    public void OnNext(EventTargetTracking value)
    {
        _isTargetVisible = value.IsVisible;
        arrow.SetActive(value.IsVisible);
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
}