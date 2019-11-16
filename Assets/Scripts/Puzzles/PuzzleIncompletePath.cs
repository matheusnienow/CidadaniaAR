using System;
using System.Transactions;
using Command;
using Enum;
using Observer;
using Puzzles.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace Puzzles
{
    public class PuzzleIncompletePath : AlwaysOnPuzzle
    {
        [SerializeField, Range(0, 1f)] private float cameraDirectionThreshold;
        [SerializeField, Range(0, 500f)] private float cameraYThreshold;
        [SerializeField, Range(0, 500f)] private float cameraLengthThreshold;
        [SerializeField] private GameObject outOfPathBlock;
        [SerializeField] private DirectionEnum outOfPathBlockDirection;
        [SerializeField] private Axis distanceAxis;
        [SerializeField] private  GameObject obstacle;

        private Camera _cam;
        private Text _text;
        private ICommand _command;

        private bool _passageAllowed;
        private bool _wasPassageAllowed;

        private new void Start()
        {
            base.Start();
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _text = GameObject.Find("DebugText")?.GetComponent<Text>();

            _command = new DeactivateEntityCommand(obstacle);
        }

        protected override bool IsConditionMet()
        {
            //var camDirection = IsCameraDirectionCorrect();
            var camDirection = PuzzleTools.IsCameraDirectionCorrect(_cam, outOfPathBlock, outOfPathBlockDirection,
                cameraDirectionThreshold);
            var camPosition = PuzzleTools.IsCameraPositionCorrect(_cam, outOfPathBlock, cameraLengthThreshold,
                cameraYThreshold, distanceAxis);

            if (_text != null) _text.text = "DIRECTION: " + camDirection + " | POSITION: " + camPosition;

            return camDirection && camPosition;
        }

        protected override void OnConditionMet()
        {
            if (!_wasPassageAllowed)
            {
                if (_text != null) _text.text = "PASSAGE ALLOWED";
                _command.Execute();
                NotifyOnNext(new EventPuzzle(EPuzzleStatus.Solved));
            }

            _wasPassageAllowed = true;
        }

        protected override void OnConditionNotMet()
        {
            if (_wasPassageAllowed)
            {
                _command.Undo();
                NotifyOnNext(new EventPuzzle(EPuzzleStatus.NotSolved));
            }

            _wasPassageAllowed = false;
        }
    }
}