using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEntityCommand : ICommand {

    public GameObject Target { get; set; }

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

        var rotationTarget = Target.transform.rotation.y == 90 ? 180 : 90;
        Target.transform.Rotate(0, rotationTarget, 0, Space.Self);
    }

    public void Undo()
    {
        Execute();
    }
}
