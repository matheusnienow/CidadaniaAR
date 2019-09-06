using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    bool isOk;
    bool wasOk;
    private GameObject textObject;
    private Text text;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        textObject = GameObject.Find("MonkeyText");
        text = textObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCam();
    }

    void CheckCam()
    {
        bool camDirection = CheckCameraDirection();
        bool camPosition = CheckCameraPosition();

        text.text = "DIRECTION: "+camDirection+" | POSITION: "+camPosition;
        //Debug.Log("Cam Direction: " + camDirection + " | Cam Position: " + camPosition);

        isOk = camDirection && camPosition;
        if (isOk)
        {
            if (!wasOk)
            {
                text.text = "MONKEY VISIBLE";
                target.GetComponent<ShowActionScript>().Execute();
            }
        }
        else
        {
            if (wasOk)
            {
                text.text = "MONKEY NOT VISIBLE";
            }
        }
        wasOk = isOk;
    }

    bool CheckCameraDirection()
    {
        Debug.DrawRay(target.transform.position, target.transform.right * 1, Color.red);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.red);

        var dot = Vector3.Dot(target.transform.right.normalized, cam.transform.forward.normalized);
        //Debug.Log("Dot product: "+dot);
        return Mathf.Abs(dot) < 1.1 && Mathf.Abs(dot) > 0.97;
    }

    bool CheckCameraPosition()
    {
        var deltaY = target.transform.position.y - cam.transform.position.y;
        var deltaZ = target.transform.position.z - cam.transform.position.z;

        //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

        bool Ypass = Mathf.Abs(deltaY) < 0.1;
        bool Zpass = Mathf.Abs(deltaZ) < 0.1;

        return Ypass && Zpass;
    }
}
