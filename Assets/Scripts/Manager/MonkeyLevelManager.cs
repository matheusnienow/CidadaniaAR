using System.Collections;
using Puzzles;
using UnityEngine;

namespace Manager
{
    [RequireComponent(typeof(PuzzleBrokenObject))]
    public class MonkeyLevelManager : LevelManager
    {
        protected override void SubscribeOnPuzzleEvents()
        {
        }

        protected override IEnumerator StartLevel()
        {
            yield return new WaitForSeconds(0);
        }

        protected override bool HandleSpecialCheckPoint(string destinationName)
        {
            switch (destinationName)
            {
                default:
                    return false;
            }
        }
    }
}