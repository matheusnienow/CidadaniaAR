using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildNavMeshCommand : Command {

    public NavMeshSurface NavMeshSurface { get; set; }

    public BuildNavMeshCommand(NavMeshSurface navMeshSurface)
    {
        NavMeshSurface = navMeshSurface;
    }

    public override void Execute()
    {
        if (NavMeshSurface != null)
        {
            NavMeshSurface.BuildNavMesh();
        }
    }

    public override void Undo()
    {
        
    }
}
