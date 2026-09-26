using UnityEngine;

namespace Project.Source
{
    public class GameFieldInfo : MonoBehaviour
    {
        public static float RightBound => Center.x + HalfWidth;
        public static float LeftBound => Center.x - HalfWidth;
        public static float TopBound => Center.y + HalfHeight;
        public static float BottomBound => Center.y - HalfHeight;
        public static float Top2BottomDistance => Camera.main.orthographicSize * 2;
        public static float HalfWidth => Camera.main.orthographicSize * Camera.main.aspect;
        public static float HalfHeight => Camera.main.orthographicSize;
        public static Vector2 Center => Camera.main.transform.position;
    }
}