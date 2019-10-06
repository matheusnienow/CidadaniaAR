namespace Puzzles.Base
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
