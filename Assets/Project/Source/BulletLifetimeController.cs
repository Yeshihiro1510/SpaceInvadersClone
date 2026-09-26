using UnityEngine;
using UnityEngine.Pool;

namespace Project.Source
{
    public class BulletLifetimeController : MonoBehaviour
    {
        private ObjectPool<BulletController> _pool;
        
        private BulletController _prefab;

        private void Awake()
        {
            _pool = new ObjectPool<BulletController>(() =>
                {
                    var bullet = Instantiate(_prefab);
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


        private void OnBulletDespawn(BulletController bullet) => _pool.Release(bullet);
        private void OnBulletDeath(BulletController bullet) => _pool.Release(bullet);

        public BulletController Spawn(BulletController prefab, Vector2 direction)
        {
            _prefab = prefab;
            var bullet = _pool.Get();
            bullet.Launch(GameFieldInfo.Top2BottomDistance / (direction * bullet.Speed).magnitude, direction);
            return bullet;
        }
    }
}