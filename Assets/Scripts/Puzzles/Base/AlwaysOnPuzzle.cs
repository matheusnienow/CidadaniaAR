using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Puzzles
{
    public abstract class AlwaysOnPuzzle : Puzzle
    { 
        protected void Update()
        {
            var isResolved = IsConditionMet();
            if (isResolved)
            {
                OnConditionMet();
            } else
            {
                OnConditionNotMet();
            }
        }
    }
}
