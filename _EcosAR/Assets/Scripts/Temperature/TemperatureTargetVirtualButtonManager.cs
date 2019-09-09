using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TemperatureTargetVirtualButtonManager : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject VirtualButtonGameObject;
    public Button TemperatureTargetHelperButton;
    public Canvas Canvas;

    private void Start()
    {
        if (VirtualButtonGameObject != null)
        {
            VirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            TemperatureTargetHelperButton.gameObject.SetActive(false);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (Canvas.isActiveAndEnabled)
        {
            TemperatureTargetHelperButton.gameObject.SetActive(true);
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        return;
    }
}