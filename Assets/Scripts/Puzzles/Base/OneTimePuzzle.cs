using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Puzzles
{
    public abstract class OneTimePuzzle : Puzzle
    {
        private bool isCompleted = false;

        protected void Update()
        {
            if (isCompleted)
            {
                return;
            }

            var isResolved = IsConditionMet();
            if (isResolved)
            {
                isCompleted = true;
                OnConditionMet();
            }
        }
    }
}
