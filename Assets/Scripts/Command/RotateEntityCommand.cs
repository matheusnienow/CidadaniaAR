using System;
using UnityEngine;

namespace Command
{
    public class RotateEntityCommand : ICommand {
        private const double Tolerance = 1;
        private GameObject Target { get; set; }

        public RotateEntityCommand(GameObject target)
        {
            Target = target;
        }

        public void Execute()
        {
            if (Target == null)
            {
                return;
            }

            var rotationTarget = Math.Abs(Target.transform.rotation.y - 90) < Tolerance ? 180 : 90;
            Target.transform.Rotate(0, rotationTarget, 0, Space.Self);
        }

        public void Undo()
        {
            Execute();
        }
    }
}
