using UnityEngine;

namespace Puzzles.Base
{
    public abstract class OneTimePuzzle : Puzzle
    {
        protected float TimeThreshold = 2f;
        private bool _isCompleted;
        private float _timer;

        protected abstract void OnIsConditionMet(float timer);
        
        protected void Update()
        {
            if (_isCompleted)
            {
                return;
            }

            var isResolved = IsConditionMet();
            if (!isResolved)
            {
                _timer = 0;
                OnConditionNotMet();
                return;
            }
            
            _timer += Time.deltaTime;
            OnIsConditionMet(_timer);
            if (_timer < TimeThreshold)
            {
                return;
            }
            
            _isCompleted = true;
            OnConditionMet();
        }
    }
}
