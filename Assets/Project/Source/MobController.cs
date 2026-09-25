using UnityEngine;

namespace Project.Source
{
    public class MobController : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float FallSpeed { get; private set; }
        
        private void Update()
        {
            var delta = -FallSpeed * Time.deltaTime;
            transform.Translate(new Vector2(0, delta));
        }
    }
}