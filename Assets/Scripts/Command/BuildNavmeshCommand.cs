using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildNavMeshCommand : ICommand {

    public NavMeshSurface NavMeshSurface { get; set; }

    public BuildNavMeshCommand(NavMeshSurface navMeshSurface)
    {
        NavMeshSurface = navMeshSurface;
    }

    public void Execute()
    {
        if (NavMeshSurface != null)
        {
            NavMeshSurface.BuildNavMesh();
        }
    }

    public void Undo()
    {
        
    }
}
