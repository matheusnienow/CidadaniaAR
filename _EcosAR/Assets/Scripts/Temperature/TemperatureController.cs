using System;
using System.Collections;
using UnityEngine;

public class TemperatureController
{
    public float Temperature { get; set; }
    public float TargetTemperature { get; set; }

    private Func<IEnumerator, Coroutine> _startCoroutine;
    private GameObject _temperatureTarget;
    private Transform _temperatureTargetTransform;
    private TemperatureTextManager _temperatureTextManager;
    private DayNightCycleController _dayNightCycleController;
    private WaitForSeconds _waitForSeconds;

    private float _lastTempFromTarget;

    public TemperatureController(Func<IEnumerator, Coroutine> StartCoroutine, DayNightCycleController dayNightCycleController)
    {
        _temperatureTextManager = new TemperatureTextManager();
        _temperatureTextManager.UpdatePanelText(0f);

        _dayNightCycleController = dayNightCycleController;

        _startCoroutine = StartCoroutine;
        _temperatureTarget = GameObject.FindGameObjectWithTag("Temperature Target");
        _temperatureTargetTransform = _temperatureTarget.transform;
        _waitForSeconds = new WaitForSeconds(1f);
        _lastTempFromTarget = 0f;

        _startCoroutine.Invoke(UpdateTemperatureRelativeToTimeOfDay());
    }

    public void Update()
    {
        UpdateTemperature();
        UpdatePanelText();
    }

    private void UpdateTemperature()
    {
        bool isBeingTracked = VuforiaTools.IsBeingTracked("Temperature Target");

        if (isBeingTracked)
        {
            var targetAngle = _temperatureTargetTransform.localRotation.eulerAngles.y;

            var mappedAngle = Map(targetAngle, 0, 280, 0, 50);

            if (_lastTempFromTarget != mappedAngle && targetAngle < 280)
            {
                Temperature = mappedAngle;

                TargetTemperature = Temperature;

                _lastTempFromTarget = Temperature;

                _temperatureTextManager.UpdateTargetText(TargetTemperature);
                _temperatureTextManager.UpdatePanelText(Temperature);
            }
        }
    }

    private void UpdatePanelText()
    {
        if (Temperature != TargetTemperature)
        {
            _temperatureTextManager.UpdatePanelText(Temperature);
        }
    }

    public IEnumerator UpdateTemperatureRelativeToTimeOfDay()
    {
        while (true)
        {
            yield return _waitForSeconds;

            LowerTempDuringNight();
            IncreaseTemperatureDuringDay();
        }
    }

    private void LowerTempDuringNight()
    {
        if (_dayNightCycleController.IsNight)
        {
            Temperature -= .5f;
        }
    }

    private void IncreaseTemperatureDuringDay()
    {
        if (_dayNightCycleController.IsDay)
        {
            Temperature += .5f;
        }
    }

    public float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}