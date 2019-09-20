using TMPro;
using UnityEngine;

public class KnobTextManager
{
    private TextMeshProUGUI _panelText;
    private TextMeshPro _targetText;
    private GameObject _knobTarget;

    private const string targetName = "KnobTarget";

    public KnobTextManager()
    {
        _knobTarget = GameObject.Find(targetName);
        _targetText = _knobTarget.GetComponentInChildren<TextMeshPro>();
    }

    public void UpdateTargetText(float angle)
    {
        _targetText.text = $"{Mathf.Round(angle)} °";
    }
}