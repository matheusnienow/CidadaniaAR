using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilderScript : MonoBehaviour, ActionScript {

    public NavMeshSurface navMeshSurface;

    public void Execute()
    {
        if (navMeshSurface != null)
        {
            Debug.Log("building navmesh");
            navMeshSurface.BuildNavMesh();
        } else
        {
            Debug.Log("navmeshsurface null");
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
