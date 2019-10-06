namespace Puzzles.Base
{
    public abstract class OneTimePuzzle : Puzzle
    {
        private bool isCompleted;

        protected void Update()
        {
            if (isCompleted)
            {
                return;
            }

            var isResolved = IsConditionMet();
            if (!isResolved)
            {
                return;
            }
            
            isCompleted = true;
            OnConditionMet();
        }
    }
}
