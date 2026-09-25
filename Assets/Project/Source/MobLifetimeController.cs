using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Project.Source
{
    public class MobLifetimeController : MonoBehaviour
    {
        [field: SerializeField] public float YTopOffset { get; private set; }
        [field: SerializeField] public float YBottomOffset { get; private set; }

        [SerializeField] private MobController _prefab;

        private ObjectPool<MobController> _pool;
        private readonly List<MobController> _activeMobs = new();
        
        private void Awake()
        {
            _pool = new ObjectPool<MobController>(() =>
                {
                    var mob = Instantiate(_prefab);
                    mob.onDespawn += OnMobDespawn;
                    return mob;
                },
                o =>
                {
                    o.gameObject.SetActive(true);
                    _activeMobs.Add(o);
                },
                o =>
                {
                    o.gameObject.SetActive(false);
                    _activeMobs.Remove(o);
                });
        }

        private void OnMobDespawn(MobController mob) => _pool.Release(mob);

        public MobController Spawn()
        {
            var mob = _pool.Get();
            mob.LifetimeTimer = (GameFieldInfo.Top2BottomDistance + YTopOffset - YBottomOffset) / mob.FallSpeed;
            mob.Health = mob.StartHealth;
            mob.transform.position = new Vector3(Random.Range(GameFieldInfo.LeftBound, GameFieldInfo.RightBound), GameFieldInfo.TopBound + YTopOffset, 0);
            return mob;
        }
    }
}