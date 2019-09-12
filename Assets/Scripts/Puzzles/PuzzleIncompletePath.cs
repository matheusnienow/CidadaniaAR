using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

public class PuzzleIncompletePath : MonoBehaviour
{
    private Camera cam;

    public GameObject outOfPathBlock;
    public GameObject invisibleBlock;
    public ActionScript action;
    public GameObject actionHolder;

    private GameObject textObject;
    private Text text;

    private bool passageAllowed;
    private bool wasPassageAllowed;

    private void Start()
    {
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        textObject = GameObject.Find("DotText");
        text = textObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckNavMesh();
    }

    void CheckNavMesh()
    {
        bool camDirection = CheckCameraDirection();
        bool camPosition = CheckCameraPosition();

        text.text = "DIRECTION: " + camDirection + " | POSITION: " + camPosition;

        //Debug.Log("Cam Direction: " + camDirection + " | Cam Position: " + camPosition);

        passageAllowed = camDirection && camPosition;
        if (passageAllowed)
        {
            if (!wasPassageAllowed)
            {
                text.text = "PASSAGE ALLOWED";
                invisibleBlock.layer = 9;//path
                actionHolder.GetComponent<ActionScript>().Execute();
            }
        }
        else
        {
            if (wasPassageAllowed)
            {
                text.text = "PASSAGE FORBIDDEN";
                invisibleBlock.layer = 0;//default
            }
        }
        wasPassageAllowed = passageAllowed;
    }

    bool CheckCameraDirection()
    {
        Debug.DrawRay(outOfPathBlock.transform.position, outOfPathBlock.transform.right * 1, Color.red);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.red);

        var dot = Vector3.Dot(outOfPathBlock.transform.right.normalized, cam.transform.forward.normalized);
        //Debug.Log("Dot product: "+dot);
        return Mathf.Abs(dot) < 1.1 && Mathf.Abs(dot) > 0.97;
    }

    bool CheckCameraPosition()
    {
        var deltaY = outOfPathBlock.transform.position.y - cam.transform.position.y;
        var deltaZ = outOfPathBlock.transform.position.z - cam.transform.position.z;

        //Debug.Log("DeltaY: "+deltaY+" | DeltaZ: "+deltaZ);

        bool Ypass = Mathf.Abs(deltaY) < 0.1;
        bool Zpass = Mathf.Abs(deltaZ) < 0.1;

        return Ypass && Zpass;
    }
}

