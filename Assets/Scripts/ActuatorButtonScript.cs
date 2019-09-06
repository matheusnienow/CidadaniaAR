using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActuatorButtonScript : MonoBehaviour {

    public GameObject Button;
    public GameObject Target;

	// Update is called once per frame
	void Update () {
        var clicked = IsClicked();
        if (clicked)
        {
            Debug.Log("activated!");
            Activate();
        }
    }

    private void Activate()
    {
        Target.GetComponent<RotateActionScript>().Execute();
    }

    bool IsClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var isButton = (ReferenceEquals(hit.transform.gameObject, Button));
                Debug.Log("hit name: "+ hit.transform.gameObject.name+" Button: "+Button.name);
                Debug.Log("raycast hit, isButton: "+isButton);
                return isButton;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}
