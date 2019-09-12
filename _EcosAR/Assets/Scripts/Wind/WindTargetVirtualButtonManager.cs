using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class WindTargetVirtualButtonManager : MonoBehaviour, IVirtualButtonEventHandler
{
    public Canvas Canvas;
    public GameObject VirtualButtonGameObject;
    public Button WindTargetHelperButton;

    void Start()
    {
        if (VirtualButtonGameObject != null)
        {
            VirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            WindTargetHelperButton.gameObject.SetActive(false);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (Canvas.isActiveAndEnabled)
        {
            WindTargetHelperButton.gameObject.SetActive(true);
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        return;
    }
}