using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEntityCommand : ICommand {

    public GameObject target;

    public ActivateEntityCommand(GameObject target)
    {
        this.target = target;
    }

    public void Execute()
    {
        if (target != null)
        {
            target.SetActive(true);
        }
    }

    public void Undo()
    {
        if (target != null)
        {
            target.SetActive(false);
        }
    }
}
