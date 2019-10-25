using Assets.Scripts.Enum;
using UnityEngine;

namespace Util
{
    public static class PuzzleTools
    {
        
        public static Vector3 GetDirection(GameObject target, DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.FORWARD:
                    return target.transform.forward;
                case DirectionEnum.BACKWARD:
                    return target.transform.forward * -1;
                case DirectionEnum.RIGHT:
                    return target.transform.right;
                case DirectionEnum.LEFT:
                    return target.transform.right * -1;
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
            Debug.Log("Block Y: "+blockY+", Cam Y: "+camY +" | Delta Y: "+deltaY);
            
            var isYAxisCorrect = Mathf.Abs(deltaY) < positionThreshold;
            return isYAxisCorrect;
        }
    }
}