using System;
using UnityEngine;

namespace Project.Source
{
    public class MobController : MonoBehaviour, IDamageable
    {
        [field: SerializeField, Min(0)] public int StartHealth { get; private set; }
        [field: SerializeField, Min(0)] public float FallSpeed { get; private set; }

        private float _timer;
        private int _health;
        private bool _isDead;

        public event Action<MobController> onDeath;
        public event Action<MobController> onDespawn;

        public void Launch(float lifetime)
        {
            _timer = lifetime;
            _health = StartHealth;
            _isDead = false;
        }

        private void Update()
        {
            var delta = -FallSpeed * Time.deltaTime;
            transform.Translate(new Vector2(0, delta));

            if (_timer <= 0) onDespawn?.Invoke(this);
            else _timer -= Time.deltaTime;
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;
            _health -= damage;
            if (_health <= 0)
            {
                _isDead = true;
                onDeath?.Invoke(this);
            }
        }
    }
}