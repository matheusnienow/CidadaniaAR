using System;
using TMPro;
using UnityEngine;

public class DayNightCycleController
{
    public bool IsNight { get; private set; }
    public bool IsDay { get; private set; }
    public float DayLengthInSeconds { get; private set; }

    private GameObject _dayInputPanel;
    private GameObject _timeInputPanel;
    private GameObject _sunAndMoonRotator;
    private float _rotationAngle;
    private double _rotationPercentage;
    private int _day;
    private int _hour;
    private int _minute;

    public DayNightCycleController()
    {
        DayLengthInSeconds = 30;
        _rotationAngle = 0;
        _rotationPercentage = 0;
        _day = 1;
        _hour = 0;
        _minute = 0;
        _dayInputPanel = GameObject.Find("DayInput");
        _timeInputPanel = GameObject.Find("TimeInput");
        _sunAndMoonRotator = GameObject.Find("Sun And Moon Rotator");
    }

    public void Update()
    {
        var degreeInSeconds = DegreeInSeconds(DayLengthInSeconds) * Time.deltaTime;

        _sunAndMoonRotator.transform.Rotate(0, 0, degreeInSeconds);

        _rotationAngle += degreeInSeconds;

        _rotationPercentage = ((_rotationAngle / 360) * -1);

        if (_rotationAngle < -360)
        {
            _rotationAngle = 0;
            _day++;
        }

        TimeOfDay();
        UpdateTextDisplays();
    }

    private double ConvertRange(int originalStart, int originalEnd, int newStart, int newEnd, double value)
    {
        double scale = (newEnd - newStart) / (originalEnd - originalStart);
        return (newStart + ((value - originalStart) * scale));
    }

    private void TimeOfDay()
    {
        double decimalTime = ConvertRange(0, 1, 0, 24, _rotationPercentage);

        _hour = (int)(decimalTime);

        _minute = (int)((decimalTime - Math.Truncate(decimalTime)) * 60);

        if ((_hour >= 18) || (_hour < 6))
        {
            IsNight = true;
            IsDay = false;
        }
        else
        {
            IsNight = false;
            IsDay = true;
        }
    }

    private float DegreeInSeconds(float seconds)
    {
        return -360 / seconds;
    }

    private void UpdateTextDisplays()
    {
        _dayInputPanel.GetComponent<TextMeshProUGUI>().text = $"{_day}";
        _timeInputPanel.GetComponent<TextMeshProUGUI>().text = $"{_hour}:{_minute}";
    }
}