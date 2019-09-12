using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowActionScript : MonoBehaviour, ActionScript {

    public GameObject target;

    public void Execute()
    {
        target.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
