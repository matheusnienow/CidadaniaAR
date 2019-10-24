using UnityEngine;

namespace Command
{
    public class DeactivateEntityCommand : ICommand {
        private GameObject Target { get; }

        public DeactivateEntityCommand(GameObject target)
        {
            Target = target;
        }

        public void Execute()
        {
            if (Target != null)
            {
                Target.SetActive(false);
            }
        }

        public void Undo()
        {
            if (Target != null)
            {
                Target.SetActive(true);
            }
        }
    }
}
