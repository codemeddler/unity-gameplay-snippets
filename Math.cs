using UnityEngine;

    public static class Math {

        public static Vector3 ConvertMouseCoordinates(Vector3 input)
        {
            Vector3 returnValue;        
            returnValue.x = (input.x - Screen.width / 2) / Screen.width;
            returnValue.y = (input.y - Screen.height / 2) / Screen.height;
            returnValue.z = input.z;        
            return returnValue;
        }
    
        public static bool IsInRange(Vector3 pointA, Vector3 pointB, float range, float threshold = 0.0f)
        {
            return (pointA - pointB).sqrMagnitude < range * range + threshold;
        }
    }

