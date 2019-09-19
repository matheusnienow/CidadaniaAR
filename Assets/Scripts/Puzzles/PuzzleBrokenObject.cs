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

    private float timer = 0.0f;

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
            }

            timer += Time.deltaTime;
            var seconds = timer % 60;
            if (seconds > 2)
            {
                target.GetComponent<ShowActionScript>().Execute();
                gameObject.SetActive(false);
            }
        }
        else
        {
            timer = 0.0f;
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
        return Mathf.Abs(dot) < 1.3 && Mathf.Abs(dot) > 0.90;
    }

    bool CheckCameraPosition()
    {
        var deltaY = brokenObject.transform.position.y - cam.transform.position.y;
        //var deltaZ = brokenObject.transform.position.z - cam.transform.position.z;

        //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

        bool Ypass = Mathf.Abs(deltaY) > 0.0 && Mathf.Abs(deltaY) < 1;
        //bool Zpass = Mathf.Abs(deltaZ) > 8.4 && Mathf.Abs(deltaZ) < 8.6;

        //return Ypass && Zpass;
        return Ypass;
    }
}
