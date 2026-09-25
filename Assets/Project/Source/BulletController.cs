using System;
using UnityEngine;

namespace Project.Source
{
    public class BulletController : MonoBehaviour
    {
        [field: SerializeField] public float UpperSpeed { get; private set; }
        
        public float LifetimeTimer { get; set; }
        
        public event Action<BulletController> onDeath;
        
        private void Update()
        {
            var delta = UpperSpeed * Time.deltaTime;
            transform.Translate(0, delta, 0);
            
            if (LifetimeTimer <= 0) Death();
            else LifetimeTimer -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Death();
        }
        
        private void Death() => onDeath?.Invoke(this);
    }
}