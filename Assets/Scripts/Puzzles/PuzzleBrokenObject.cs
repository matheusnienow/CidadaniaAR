using UnityEngine;
using UnityEngine.UI;

public class PuzzleBrokenObject : MonoBehaviour
{
    private Camera cam;

    public Transform brokenObject;
    public Transform target;

    bool isOk;
    bool wasOk;

    private GameObject textObject;
    private GameObject textHintObject;
    private Text textMonkey;
    private Text textHint;

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        textObject = GameObject.Find("MonkeyText");
        textHintObject = GameObject.Find("DotText");
        textMonkey = textObject.GetComponent<Text>();
        textHint = textHintObject.GetComponent<Text>();
    }

    void Update()
    {
        CheckCam();
    }

    void CheckCam()
    {
        bool camDirection = CheckCameraDirection();
        bool camPosition = CheckCameraPosition();

        textMonkey.text = "TDIRECTION: "+camDirection+" | POSITION: "+camPosition+" OBJECT: "+ brokenObject.name;
        //Debug.Log("Cam Direction: " + camDirection + " | Cam Position: " + camPosition);

        isOk = camDirection && camPosition;
        if (isOk)
        {
            if (!wasOk)
            {
                textMonkey.text = "MONKEY VISIBLE (target: "+ brokenObject.name+")";
                target.GetComponent<ShowActionScript>().Execute();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (wasOk)
            {
                textMonkey.text = "MONKEY NOT VISIBLE";
            }
        }
        wasOk = isOk;
    }

    bool CheckCameraDirection()
    {
        //Debug.DrawRay(target.transform.position, target.transform.forward * 1, Color.red);
        //Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.red);

        var dot = Vector3.Dot(brokenObject.transform.forward.normalized, cam.transform.forward.normalized);
        //Debug.Log("Dot product: "+dot);
        return Mathf.Abs(dot) < 1.1 && Mathf.Abs(dot) > 0.97;
    }

    bool CheckCameraPosition()
    {
        var deltaY = brokenObject.transform.position.y - cam.transform.position.y;
        var deltaZ = brokenObject.transform.position.z - cam.transform.position.z;

        //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

        bool Ypass = Mathf.Abs(deltaY) > 0.0 && Mathf.Abs(deltaY) < 0.4;
        bool Zpass = Mathf.Abs(deltaZ) > 8.4 && Mathf.Abs(deltaZ) < 8.6;

        return Ypass && Zpass;
    }
}
