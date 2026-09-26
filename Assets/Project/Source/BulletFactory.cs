using UnityEngine;
using UnityEngine.Pool;

namespace Project.Source
{
    public static class BulletFactory
    {
        private static readonly ObjectPool<BulletController> _pool;
        
        private static BulletController _prefab;

        static BulletFactory()
        {
            _pool = new ObjectPool<BulletController>(() =>
                {
                    var bullet = Object.Instantiate(_prefab);
                    bullet.onDespawn += OnBulletDespawn;
                    bullet.onDeath += OnBulletDeath;
                    return bullet;
                },
                o => o.gameObject.SetActive(true), 
                o => o.gameObject.SetActive(false),
                o =>
                {
                    o.onDespawn -= OnBulletDespawn;
                    o.onDeath += OnBulletDeath;
                });
        }

        private static void OnBulletDespawn(BulletController bullet) => _pool.Release(bullet);
        private static void OnBulletDeath(BulletController bullet) => _pool.Release(bullet);

        public static BulletController Spawn(BulletController prefab, Vector2 direction)
        {
            _prefab = prefab;
            var bullet = _pool.Get();
            bullet.Launch(GameFieldInfo.Top2BottomDistance / (direction * bullet.Speed).magnitude, direction);
            return bullet;
        }
    }
}