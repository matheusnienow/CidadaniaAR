using UnityEngine.AI;

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
                NavMeshSurface.BuildNavMesh();
            }
        }

        public void Undo()
        {
        
        }
    }
}
