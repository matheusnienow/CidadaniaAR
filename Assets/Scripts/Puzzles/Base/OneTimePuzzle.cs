namespace Puzzles.Base
{
    public abstract class OneTimePuzzle : Puzzle
    {
        private bool _isCompleted;

        protected void Update()
        {
            if (_isCompleted)
            {
                return;
            }

            var isResolved = IsConditionMet();
            if (!isResolved)
            {
                return;
            }
            
            _isCompleted = true;
            OnConditionMet();
        }
    }
}
