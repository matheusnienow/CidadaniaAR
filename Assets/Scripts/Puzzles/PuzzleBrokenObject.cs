using System.Transactions;
using Assets.Scripts.Observer;
using Command;
using Enum;
using Observer;
using Puzzles.Base;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Puzzles
{
    public class PuzzleBrokenObject : OneTimePuzzle
    {
        private Camera _cam;

        public GameObject brokenObject;
        [SerializeField]
        public DirectionEnum brokenObjectDirection;
        public GameObject obstacle;
        public float secondsToUnlock;
        [SerializeField, Range(0, 1f)]
        public float directionThreshold;
        [SerializeField, Range(0, 500f)]
        public float positionThreshold;
        
        private bool _isOk;
        private bool _wasOk;
        private Text _text;

        private ICommand _command;

        private new void Start()
        {
            base.Start();
            _cam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
            _text = GameObject.Find("InfoText")?.GetComponent<Text>();
            _command = new DeactivateEntityCommand(obstacle);
            TimeThreshold = 2f;
        }

        protected override bool IsConditionMet()
        {

            //var camDirection = IsCameraDirectionCorrect();
            var camDirection = PuzzleTools.IsCameraDirectionCorrect(_cam, brokenObject, brokenObjectDirection,
                directionThreshold);
            var camPosition = PuzzleTools.IsCameraPositionCorrect(_cam, brokenObject, positionThreshold);

            if (_text != null) _text.text = "DIRECTION: " + camDirection + " | POSITION: " + camPosition;

            return camDirection && camPosition;
        }

        protected override void OnConditionMet()
        {
            if (!_wasOk)
            {
                _text.text = "PUZZLE UNLOCKED";
                _command.Execute();
                NotifyOnNext(new EventPuzzle(EPuzzleStatus.Solved, true));
            }
            
            _wasOk = true;
        }

        protected override void OnConditionNotMet()
        {
            if (_wasOk)
            {
                _text.text = "MONKEY NOT VISIBLE";
                NotifyOnNext(new EventPuzzle(EPuzzleStatus.NotSolved));
            }
            
            _wasOk = false;
        }

        protected override void OnIsConditionMet(float timer)
        {
            NotifyOnNext(new EventPuzzle(EPuzzleStatus.InProgress));
            _text.text = "MONKEY VISIBLE ("+timer+")";
        }
    }
}
