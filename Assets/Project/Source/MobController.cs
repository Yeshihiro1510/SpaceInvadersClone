using System;
using UnityEngine;

namespace Project.Source
{
    public class MobController : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public int StartHealth { get; private set; }
        [field: SerializeField, Min(0)] public float FallSpeed { get; private set; }
        
        public float LifetimeTimer { get; set; }
        public float Health { get; set; }

        public event Action onDeath;
        public event Action<MobController> onDespawn;
        
        private void Update()
        {
            var delta = -FallSpeed * Time.deltaTime;
            transform.Translate(new Vector2(0, delta));
            
            if (LifetimeTimer <= 0) onDespawn?.Invoke(this);
            else LifetimeTimer -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Health <= 0)
            {
                onDeath?.Invoke();
                onDespawn?.Invoke(this);
            }
            else Health--;
        }
    }
}