using UnityEngine;
using UnityEngine.Pool;

namespace Project.Source
{
    public class BulletFactory
    {
        private readonly BulletController _playerBulletPrefab = Resources.Load<BulletController>("PlayerBullet");
        private readonly BulletController _mobBulletPrefab = Resources.Load<BulletController>("MobBullet");
        
        private readonly ObjectPool<BulletController> _playerBulletsPool;
        private readonly ObjectPool<BulletController> _mobBulletsPool;
        
        public BulletFactory()
        {
            _playerBulletsPool = new ObjectPool<BulletController>(() =>
                {
                    var bullet = Object.Instantiate(_playerBulletPrefab);
                    bullet.onDespawn += OnPlayerBulletDespawn;
                    bullet.onDeath += OnPlayerBulletDeath;
                    return bullet;
                },
                o => o.gameObject.SetActive(true),
                o => o.gameObject.SetActive(false),
                o =>
                {
                    o.onDespawn -= OnPlayerBulletDespawn;
                    o.onDeath -= OnPlayerBulletDeath;
                }
            );

            _mobBulletsPool = new ObjectPool<BulletController>(() =>
                {
                    var bullet = Object.Instantiate(_mobBulletPrefab);
                    bullet.onDespawn += OnMobBulletDespawn;
                    bullet.onDeath += OnMobBulletDeath;
                    return bullet;
                },
                o => o.gameObject.SetActive(true),
                o => o.gameObject.SetActive(false),
                o =>
                {
                    o.onDespawn -= OnMobBulletDespawn;
                    o.onDeath -= OnMobBulletDeath;
                }
            );
        }

        private void OnPlayerBulletDespawn(BulletController bullet) => _playerBulletsPool.Release(bullet);
        private void OnPlayerBulletDeath(BulletController bullet) => _playerBulletsPool.Release(bullet);
        private void OnMobBulletDespawn(BulletController bullet) => _mobBulletsPool.Release(bullet);
        private void OnMobBulletDeath(BulletController bullet) => _mobBulletsPool.Release(bullet);

        public BulletController CreatePlayerBullet(Vector2 direction)
        {
            var bullet = _playerBulletsPool.Get();
            bullet.Launch(GameFieldInfo.Top2BottomDistance / bullet.Speed, direction);
            return bullet;
        }

        public BulletController CreateMobBullet(Vector2 direction)
        {
            var bullet = _mobBulletsPool.Get();
            bullet.Launch(GameFieldInfo.Top2BottomDistance / bullet.Speed, direction);
            return bullet;            
        }
    }
}