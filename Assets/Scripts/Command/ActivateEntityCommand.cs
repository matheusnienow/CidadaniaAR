using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEntityCommand : Command {

    public GameObject target;

    public ActivateEntityCommand(GameObject target)
    {
        this.target = target;
    }

    public override void Execute()
    {
        if (target != null)
        {
            target.SetActive(true);
        }
    }

    public override void Undo()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
    }
}
