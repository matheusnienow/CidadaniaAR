using System;
using Enum;
using UnityEngine;

namespace Util
{
    public static class PuzzleTools
    {
        private static Vector3 GetDirection(GameObject target, Direction direction)
        {
            switch (direction)
            {
                case Direction.Forward:
                    return target.transform.forward;
                case Direction.Backward:
                    return target.transform.forward * -1;
                case Direction.Right:
                    return target.transform.right;
                case Direction.Left:
                    return target.transform.right * -1;
                case Direction.Up:
                    return target.transform.up;
                case Direction.Down:
                    return target.transform.up * -1;
                default:
                    return target.transform.forward;
            }
        }

        public static bool IsCameraDirectionCorrect(Camera cam, GameObject outOfPathBlock,
            Direction outOfPathBlockDirection, float directionThreshold)
        {
            var camDirectionVector = cam.transform.forward;
            var outOfPathBlockDirectionVector = GetDirection(outOfPathBlock, outOfPathBlockDirection);

            Debug.DrawRay(outOfPathBlock.transform.position, outOfPathBlockDirectionVector * 1000, Color.magenta);

            var dotAbs = Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized) * -1;

            var maxValue = (1 + directionThreshold);
            var minValue = (1 - directionThreshold);

            return dotAbs < maxValue && dotAbs > minValue;
        }

        public static bool IsCameraPositionCorrect(Camera cam, GameObject outOfPathBlock, float cameraXThreshold,
            float cameraYThreshold, Axis axis)
        {
            var camPosition = cam.gameObject.transform.position;
            var blockPosition = GetPosition(outOfPathBlock);

            var deltaLength = axis == Axis.X
                ? AbsDifference(camPosition.z, blockPosition.z)
                : AbsDifference(camPosition.x, blockPosition.x);

            Debug.Log("deltaLength: " + deltaLength);

            var deltaY = AbsDifference(camPosition.y, blockPosition.y);

            var isYAxisCorrect = deltaY < cameraYThreshold;
            var isLengthAxisCorrect = deltaLength < cameraXThreshold;

            return isYAxisCorrect && isLengthAxisCorrect;
        }

        public static Vector3 GetPosition(GameObject gameObject)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            return renderer != null ? renderer.bounds.center : gameObject.transform.position;
        }

        private static float AbsDifference(float n1, float n2)
        {
            var delta = n1 - n2;
            return Mathf.Abs(delta);
        }

        public static Vector3 GetGameObjectBase(GameObject g)
        {
            var center = g.GetComponent<Renderer>().bounds.center;
            var y = g.transform.position.y;

            return new Vector3(center.x, y, center.z);
        }
    }
}