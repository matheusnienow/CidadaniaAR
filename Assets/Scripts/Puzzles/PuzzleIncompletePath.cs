using System;
using System.Transactions;
using Assets.Scripts.Enum;
using Assets.Scripts.Observer;
using Command;
using Puzzles.Base;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Puzzles
{
    public class PuzzleIncompletePath : OneTimePuzzle
    {
        [FormerlySerializedAs("CameraDirectionThreshold")] [SerializeField, Range(0, 1f)]
        public float cameraDirectionThreshold;

        [FormerlySerializedAs("CameraPositionThreshold")] [SerializeField, Range(0, 1f)]
        public float cameraPositionThreshold;

        public GameObject outOfPathBlock;
        [SerializeField]
        public DirectionEnum outOfPathBlockDirection;

        public GameObject invisibleBlock;
        [FormerlySerializedAs("NavMesh")] public GameObject navMesh;

        private Camera _cam;
        private Text _text;    
        private ICommand _buildNavMeshCommand;

        private bool _passageAllowed;
        private bool _wasPassageAllowed;

        private new void Start()
        {
            base.Start();
            _cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _text = GameObject.Find("DebugText")?.GetComponent<Text>();

            var navMeshSurface = navMesh.GetComponent<NavMeshSurface>();
            _buildNavMeshCommand = new BuildNavMeshCommand(navMeshSurface);
        }

        private bool IsCameraDirectionCorrect()
        {
            var camDirectionVector = _cam.transform.forward;
            var outOfPathBlockDirectionVector = GetDirection(outOfPathBlock, outOfPathBlockDirection);

            Debug.DrawRay(outOfPathBlock.transform.position, outOfPathBlockDirectionVector * 10, Color.red);
            Debug.DrawRay(_cam.transform.position, camDirectionVector * 10, Color.red);

            var dotAbs = Mathf.Abs(Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized));

            var maxValue = (1 + cameraDirectionThreshold);
            var minValue = (1 - cameraDirectionThreshold);

            return dotAbs < maxValue && dotAbs > minValue;
        }

        private Vector3 GetDirection(GameObject blockObject, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.FORWARD:
                    return blockObject.transform.forward;
                case DirectionEnum.BACKWARD:
                    return blockObject.transform.forward * -1;
                case DirectionEnum.RIGHT:
                    return blockObject.transform.right;
                case DirectionEnum.LEFT:
                    return blockObject.transform.right * -1;
                default:
                    return blockObject.transform.forward;
            }
        }

        private bool IsCameraPositionCorrect()
        {
            var deltaY = outOfPathBlock.transform.position.y - _cam.transform.position.y;
            var isYAxisCorrect = Mathf.Abs(deltaY) < cameraPositionThreshold;

            return isYAxisCorrect;
        }

        protected override bool IsConditionMet()
        {
            var camDirection = IsCameraDirectionCorrect();
            var camPosition = IsCameraPositionCorrect();

            //Debug.Log("DIRECTION: " + camDirection + " | POSITION: " + camPosition);

            return camDirection && camPosition;
        }

        protected override void OnConditionMet()
        {
            if (!_wasPassageAllowed)
            {
                if (_text!= null) _text.text = "PASSAGE ALLOWED";
                BuildNavMesh(9);
            }

            _wasPassageAllowed = true;
        }

        protected override void OnConditionNotMet()
        {
            if (_wasPassageAllowed)
            {
                if (_text!= null) _text.text = "PASSAGE NOT ALLOWED";
                BuildNavMesh(0);
            }

            _wasPassageAllowed = false;
        }

        private void BuildNavMesh(int layer)
        {
            invisibleBlock.layer = layer;
            _buildNavMeshCommand.Execute();
        }

        protected override Message GetOnNextMessage()
        {
            throw new NotImplementedException();
        }
    }
}

