using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobTargetController : MonoBehaviour
{
    public float Angle { get; set; }
    public float TargetAngle { get; set; }

    public GameObject KnobSubject;

    private GameObject _knob;
    private Transform _knobTransform;
    private KnobTextManager textManager;
    private WaitForSeconds _waitForSeconds;

    private float _lastAngleFromTarget;

    private const string targetName = "KnobTarget";

    public void Start()
    {
        textManager = new KnobTextManager();

        _knob = GameObject.Find(targetName);
        _knobTransform = _knob.transform;
        _waitForSeconds = new WaitForSeconds(1f);
        _lastAngleFromTarget = 0f;
    }

    public void Update()
    {
        UpdateAngle();
    }

    private void UpdateAngle()
    {
        bool isBeingTracked = VuforiaTools.IsBeingTracked(targetName);
        if (isBeingTracked)
        {
            Angle = _knobTransform.rotation.eulerAngles.y;
            //var mappedAngle = Map(targetAngle, 0, 280, 0, 50);

            TargetAngle = Angle;
            _lastAngleFromTarget = Angle;

            textManager.UpdateTargetText(TargetAngle);
            RotateSubject(Angle);
        }
    }

    private void RotateSubject(float Angle)
    {
        var subjectRotation = KnobSubject.transform.eulerAngles;
        subjectRotation.y = Angle;
        KnobSubject.transform.Rotate(subjectRotation, Space.Self);
    }

    public float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}