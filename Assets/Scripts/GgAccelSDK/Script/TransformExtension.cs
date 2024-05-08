using UnityEngine;

namespace GgAccel
{
    public static class TransformExtension
    {
        /**
         *Get direction from this to target object;
         */
        public static Vector3 GetDirection(this Transform transform, Vector3 target)
        {
            return (target - transform.position).normalized;
        }

        public static Vector2 GetDirection(this Transform transform, Vector2 target)
        {
            return (target - (Vector2)transform.position).normalized;
        }

        public static float GetMagnitude(this Transform transform, Vector3 target)
        {
            return (target - transform.position).sqrMagnitude;
        }
        
        public static Vector3 AThirdOfCircle(this Transform transform)
        {
            return transform.rotation * new Vector3(Mathf.Sin(2 * Mathf.PI / 3), Mathf.Cos(2 * Mathf.PI / 3));
        }

        public static Vector3 TwoThirdOfCircle(this Transform transform)
        {
            return transform.rotation * new Vector3(Mathf.Sin(4 * Mathf.PI / 3), Mathf.Cos(4 * Mathf.PI / 3));
        }
    }
}