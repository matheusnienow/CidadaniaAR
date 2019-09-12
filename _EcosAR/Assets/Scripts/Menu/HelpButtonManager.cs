using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class HelpButtonManager : MonoBehaviour
{
    public Button HelpTextButton;
    public Button TemperatureTargetTextButton;
    public Button WindTargetTextButton;
    public Button SceneTargetTextButton;
    public GameObject WindTargetVirtualButtonGameObject;
    public GameObject TemperatureTargetVirtualButtonGameObject;
    public GameObject SceneTargetVirtualButtonGameObject;

    private bool _virtualButtonsEnabled;

    void Start()
    {
        HelpTextButton.gameObject.SetActive(false);

        SceneTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = false;
        WindTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = false;
        TemperatureTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = false;
        _virtualButtonsEnabled = false;
    }

    public void ToggleVirtualButtons()
    {
        SceneTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = (!SceneTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled);
        WindTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = (!WindTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled);
        TemperatureTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled = (!TemperatureTargetVirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().enabled);

        if (!_virtualButtonsEnabled)
        {
            HelpTextButton.gameObject.SetActive(true);
        }
        else
        {
            HelpTextButton.gameObject.SetActive(false);

            TemperatureTargetTextButton.gameObject.SetActive(false);
            WindTargetTextButton.gameObject.SetActive(false);
            SceneTargetTextButton.gameObject.SetActive(false);
        }

        _virtualButtonsEnabled = !_virtualButtonsEnabled;
    }
}