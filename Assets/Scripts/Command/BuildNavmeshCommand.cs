using System.Diagnostics;
using UnityEngine.AI;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Command
{
    public class BuildNavMeshCommand : ICommand {
        private NavMeshSurface NavMeshSurface { get; set; }

        public BuildNavMeshCommand(NavMeshSurface navMeshSurface)
        {
            NavMeshSurface = navMeshSurface;
        }

        public void Execute()
        {
            if (NavMeshSurface != null)
            {
                Debug.Log("BUILDING NAVMESH!");
                NavMeshSurface.BuildNavMesh();
            }
        }

        public void Undo()
        {
        
        }
    }
}
