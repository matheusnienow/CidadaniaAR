using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class KnobTargetVirtualButtonManager : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject VirtualButtonGameObject;
    public Button knobTargetHelperButton;
    public Canvas Canvas;

    private void Start()
    {
        if (VirtualButtonGameObject != null)
        {
            VirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            knobTargetHelperButton.gameObject.SetActive(false);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (Canvas.isActiveAndEnabled)
        {
            knobTargetHelperButton.gameObject.SetActive(true);
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        return;
    }
}