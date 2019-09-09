using System.Collections;
using UnityEngine;

public class CloseButtonAfterSeconds : MonoBehaviour
{
    private GameObject _helpTextButton;
    private GameObject _sceneTargetButton;
    private GameObject _windTargetButton;
    private GameObject _temperatureTargetButton;
    private WaitForSeconds _waitForSeconds;

    private bool _firstEnableExecuted;

    private void Awake()
    {
        _helpTextButton = GameObject.Find("Help Text Button");
        _sceneTargetButton = GameObject.Find("Scene Target Button");
        _windTargetButton = GameObject.Find("Wind Target Help Button");
        _temperatureTargetButton = GameObject.Find("Temperature Target Help Button");

        _waitForSeconds = new WaitForSeconds(8f);
        _firstEnableExecuted = false;
    }

    void OnEnable()
    {
        if (!_firstEnableExecuted)
        {
            _firstEnableExecuted = true;
        }
        else
        {
            CloseOtherButtons();
            CloseAfterSeconds();
        }
    }

    void CloseAfterSeconds()
    {
        StartCoroutine(CloseButtonAfterFiveSeconds());
    }

    IEnumerator CloseButtonAfterFiveSeconds()
    {
        yield return _waitForSeconds;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    void CloseOtherButtons()
    {
        string currentButtonName = gameObject.name;

        if (currentButtonName.Equals(_sceneTargetButton.name))
        {
            if (_helpTextButton.gameObject.activeSelf)
            {
                _helpTextButton.gameObject.SetActive(false);
            }
            if (_temperatureTargetButton.gameObject.activeSelf)
            {
                _temperatureTargetButton.gameObject.SetActive(false);
            }
            if (_windTargetButton.gameObject.activeSelf)
            {
                _windTargetButton.gameObject.SetActive(false);
            }
        }
        else if (currentButtonName.Equals(_temperatureTargetButton.name))
        {
            if (_helpTextButton.gameObject.activeSelf)
            {
                _helpTextButton.gameObject.SetActive(false);
            }
            if (_sceneTargetButton.gameObject.activeSelf)
            {
                _sceneTargetButton.gameObject.SetActive(false);
            }
            if (_windTargetButton.gameObject.activeSelf)
            {
                _windTargetButton.gameObject.SetActive(false);
            }
        }
        else if (currentButtonName.Equals(_windTargetButton.name))
        {
            if (_helpTextButton.gameObject.activeSelf)
            {
                _helpTextButton.gameObject.SetActive(false);
            }
            if (_temperatureTargetButton.gameObject.activeSelf)
            {
                _temperatureTargetButton.gameObject.SetActive(false);
            }
            if (_sceneTargetButton.gameObject.activeSelf)
            {
                _sceneTargetButton.gameObject.SetActive(false);
            }
        }
    }
}