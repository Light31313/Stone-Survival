using UnityEngine;

namespace GgAccel
{
    public static class CameraHelper
    {
        private static Camera _camera;

        public static Camera GameCamera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, GameCamera, out var result);
            return result;
        }

        public static Vector2 ScreenToWorldPoint(Vector3 position)
        {
            return GameCamera.ScreenToWorldPoint(position);
        }

        public static void SetOrthographicSize(float size)
        {
            GameCamera.orthographicSize = size;
        }

        public static Vector2 GetMousePositionInWorldPoint()
        {
            return GameCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public static Ray ScreenPointToRay(Vector3 pos)
        {
            return GameCamera.ScreenPointToRay(pos);
        }
    }
}