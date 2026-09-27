using UnityEngine;

namespace Project.Source
{
    public class GameFieldInfo : MonoBehaviour
    {
        public static Vector2 Center { get; private set; }
        public static float HalfWidth { get; private set; }
        public static float HalfHeight { get; private set; }
        public static float RightBound { get; private set; }
        public static float LeftBound { get; private set; }
        public static float TopBound { get; private set; }
        public static float BottomBound { get; private set; }
        public static float Top2BottomDistance { get; private set; }

        private void Awake()
        {
            if (!TryGetComponent(out Camera camera)) return;
            
            Center = camera.transform.position;
            HalfHeight = camera.orthographicSize;
            HalfWidth = camera.orthographicSize * camera.aspect;

            RightBound = Center.x + HalfWidth;
            LeftBound = Center.x - HalfWidth;
            TopBound = Center.y + HalfHeight;
            BottomBound = Center.y - HalfHeight;

            Top2BottomDistance = TopBound - BottomBound;
        }
    }
}