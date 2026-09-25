using UnityEngine;

namespace Project.Source
{
    public class GameFieldInfo : MonoBehaviour
    {
        public static float LeftBound { get; private set; }
        public static float RightBound { get; private set; }
        public static float TopBound { get; private set; }
        public static float BottomBound { get; private set; }
        public static float Top2BottomDistance { get; private set; }

        private void OnValidate()
        {
            var xHalfSize = transform.localScale.x / 2f;
            var yHalfSize = transform.localScale.y / 2f;
            
            RightBound = transform.position.x + xHalfSize;
            LeftBound = transform.position.x - xHalfSize;
            TopBound = transform.position.y + yHalfSize;
            BottomBound = transform.position.y - yHalfSize;
            Top2BottomDistance = TopBound - BottomBound;
        }
    }
}