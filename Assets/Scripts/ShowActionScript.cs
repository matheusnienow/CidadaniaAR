using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowActionScript : MonoBehaviour, ActionScript {
    
    public void Execute()
    {
        gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
