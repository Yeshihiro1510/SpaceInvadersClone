using UnityEngine;
using UnityEngine.Pool;

namespace Project.Source
{
    public class BulletLifetimeController : MonoBehaviour
    {
        [SerializeField] private BulletController _prefab;
        
        private ObjectPool<BulletController> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<BulletController>(() =>
                {
                    var bullet = Instantiate(_prefab);
                    bullet.onDeath += OnBulletDeath;
                    return bullet;
                },
                o => o.gameObject.SetActive(true), 
                o => o.gameObject.SetActive(false));
        }

        private void OnBulletDeath(BulletController bullet) => _pool.Release(bullet);

        public BulletController Spawn()
        {
            var bullet = _pool.Get();
            bullet.LifetimeTimer = GameFieldInfo.Top2BottomDistance / bullet.UpperSpeed;
            return bullet;
        }
    }
}