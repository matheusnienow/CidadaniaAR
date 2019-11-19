using System.Transactions;
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
        public GameObject brokenObject;
        public GameObject obstacle;

        [SerializeField] public Direction brokenObjectDirection;
        [SerializeField] private Axis distanceAxis;

        [SerializeField, Range(0, 1f)] public float directionThreshold;
        [SerializeField, Range(0, 5000f)] public float positionLengthThreshold;
        [SerializeField, Range(0, 500f)] public float positionYThreshold;

        [SerializeField] public bool log;

        private Camera _cam;
        private bool _isOk;
        private bool _wasOk;
        private bool _wasVisible;
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
            var camDirection = PuzzleTools.IsCameraDirectionCorrect(_cam, brokenObject, brokenObjectDirection,
                directionThreshold);
            var camPosition =
                PuzzleTools.IsCameraPositionCorrect(_cam, brokenObject, positionLengthThreshold, positionYThreshold,
                    distanceAxis);

            if (_text != null && log) _text.text = "DIRECTION: " + camDirection + " | POSITION: " + camPosition;

            return camDirection && camPosition;
        }

        protected override void OnConditionMet()
        {
            if (!_wasOk)
            {
                if (_text != null) _text.text = "PUZZLE UNLOCKED";
                _command.Execute();
                NotifyOnNext(new EventPuzzle(PuzzleStatus.Solved, true, brokenObject.name));
            }

            _wasOk = true;
        }

        protected override void OnConditionNotMet()
        {
            if (_wasVisible)
            {
                //_text.text = "MONKEY NOT VISIBLE";
                NotifyOnNext(new EventPuzzle(PuzzleStatus.NotSolved));
            }

            _wasOk = false;
        }

        protected override void OnIsConditionMet(float timer)
        {
            _wasVisible = true;
            NotifyOnNext(new EventPuzzle(PuzzleStatus.InProgress));
            if (_text != null) _text.text = "MONKEY VISIBLE (" + timer + ")";
        }
    }
}