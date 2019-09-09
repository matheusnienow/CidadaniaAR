using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class SceneTargetVirtualButtonManager : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject VirtualButtonGameObject;
    public Button SceneTargetHelperButton;
    public Canvas Canvas;

    private void Start()
    {
        if (VirtualButtonGameObject != null)
        {
            VirtualButtonGameObject.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            SceneTargetHelperButton.gameObject.SetActive(false);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (Canvas.isActiveAndEnabled)
        {
            SceneTargetHelperButton.gameObject.SetActive(true);
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        return;
    }
}
