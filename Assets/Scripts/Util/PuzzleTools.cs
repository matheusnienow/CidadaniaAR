using Enum;
using UnityEngine;

namespace Util
{
    public static class PuzzleTools
    {
        private static Vector3 GetDirection(GameObject target, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.Forward:
                    return target.transform.forward;
                case DirectionEnum.Backward:
                    return target.transform.forward * -1;
                case DirectionEnum.Right:
                    return target.transform.right;
                case DirectionEnum.Left:
                    return target.transform.right * -1;
                case DirectionEnum.Up:
                    return target.transform.up;
                case DirectionEnum.Down:
                    return target.transform.up * -1;
                default:
                    return target.transform.forward;
            }
        }

        public static bool IsCameraDirectionCorrect(Camera cam, GameObject outOfPathBlock,
            DirectionEnum outOfPathBlockDirection, float directionThreshold)
        {
            var camDirectionVector = cam.transform.forward;
            var outOfPathBlockDirectionVector = GetDirection(outOfPathBlock, outOfPathBlockDirection);

            var dotAbs = Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized) * -1;

            var maxValue = (1 + directionThreshold);
            var minValue = (1 - directionThreshold);

            return dotAbs < maxValue && dotAbs > minValue;
        }

        public static float GetDirectionDifference(GameObject cam, GameObject block, DirectionEnum direction)
        {
            var camDirectionVector = cam.transform.forward;
            var outOfPathBlockDirectionVector = GetDirection(block, direction);

            var dotAbs = Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized);
            return dotAbs;
        }

        public static bool IsCameraPositionCorrect(Camera cam, GameObject outOfPathBlock, float cameraXThreshold,
            float cameraYThreshold)
        {
            var camPosition = cam.gameObject.transform.position;
            var blockPosition = GetPosition(outOfPathBlock);

            var deltaX = GetXDifference(camPosition, blockPosition);
            var deltaY = GetYDifference(camPosition, blockPosition);

            var isYAxisCorrect = deltaY < cameraYThreshold;
            var isXAxisCorrect = deltaX < cameraXThreshold;

            return isYAxisCorrect && isXAxisCorrect;
        }

        public static Vector3 GetPosition(GameObject gameObject)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            return renderer != null ? renderer.bounds.center : gameObject.transform.position;
        }

        private static float GetXDifference(Vector3 cam, Vector3 block)
        {
            var camX = cam.x;
            var blockX = block.x;

            return AbsDifference(blockX, camX);
        }

        private static float GetYDifference(Vector3 cam, Vector3 block)
        {
            var camY = cam.y;
            var blockY = block.y;

            return AbsDifference(blockY, camY);
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