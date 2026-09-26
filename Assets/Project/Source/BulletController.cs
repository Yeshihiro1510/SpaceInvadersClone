using System;
using UnityEngine;

namespace Project.Source
{
    public class BulletController : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        private float _timer;
        private Vector2 _velocity;

        public event Action<BulletController> onDespawn;
        public event Action<BulletController> onDeath;

        public void Launch(float lifetime, Vector2 direction)
        {
            _timer = lifetime;
            _velocity = direction * Speed;
        }

        private void Update()
        {
            transform.Translate(_velocity * Time.deltaTime);

            if (_timer <= 0) onDespawn?.Invoke(this);
            else _timer -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
                onDeath?.Invoke(this);
            }
        }
    }
}