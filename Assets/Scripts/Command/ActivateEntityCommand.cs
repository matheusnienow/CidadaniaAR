using System.Collections;
using System.Collections.Generic;
using Command;
using UnityEngine;

public abstract class ActivateEntityCommand : ICommand {
    private GameObject Target { get; }

    public ActivateEntityCommand(GameObject target)
    {
        this.Target = target;
    }

    public void Execute()
    {
        if (Target != null)
        {
            Target.SetActive(true);
        }
    }

    public void Undo()
    {
        if (Target != null)
        {
            Target.SetActive(false);
        }
    }
}
