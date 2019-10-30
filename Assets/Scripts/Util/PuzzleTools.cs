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
        
        public static bool IsCameraDirectionCorrect(Camera cam, GameObject outOfPathBlock, DirectionEnum outOfPathBlockDirection, float directionThreshold)
        {
            var camDirectionVector = cam.transform.forward;
            var outOfPathBlockDirectionVector = GetDirection(outOfPathBlock, outOfPathBlockDirection);

            var dotAbs = Mathf.Abs(Vector3.Dot(outOfPathBlockDirectionVector.normalized, camDirectionVector.normalized));

            var maxValue = (1 + directionThreshold);
            var minValue = (1 - directionThreshold);

            return dotAbs < maxValue && dotAbs > minValue;
        }
        
        public static bool IsCameraPositionCorrect(Camera cam, GameObject outOfPathBlock, float positionThreshold)
        {
            var camY = cam.transform.position.y;
            var blockY = outOfPathBlock.transform.position.y;
            var deltaY = blockY - camY;
            
            var isYAxisCorrect = Mathf.Abs(deltaY) < positionThreshold;
            return isYAxisCorrect;
        }
    }
}