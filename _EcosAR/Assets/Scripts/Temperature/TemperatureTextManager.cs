using TMPro;
using UnityEngine;

public class TemperatureTextManager
{
    private TextMeshProUGUI _panelText;

    private TextMeshPro _targetText;

    private GameObject _temperatureTarget;

    private GameObject _temperaturePanelText;

    public TemperatureTextManager()
    {
        _temperatureTarget = GameObject.Find("Temperature Target");

        _targetText = _temperatureTarget.GetComponentInChildren<TextMeshPro>();

        _temperaturePanelText = GameObject.Find("TemperaturePanelText");

        _panelText = _temperaturePanelText.GetComponent<TextMeshProUGUI>();
    }

    public void UpdatePanelText(float temperature)
    {
        _panelText.text = $"{Mathf.Round(temperature)} °C";
    }

    public void UpdateTargetText(float temperature)
    {
        _targetText.text = $"{Mathf.Round(temperature)} °C";
    }
}