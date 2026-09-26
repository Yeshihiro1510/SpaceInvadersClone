using System;
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
        [SerializeField] private Grid _grid;

        private ObjectPool<MobController> _pool;
        private readonly List<MobController> _activeMobs = new();
        
        public int ActiveMobCount => _activeMobs.Count;

        public event Action onMobDeath;
        public event Action onMobDespawn;

        private void Awake()
        {
            _pool = new ObjectPool<MobController>(() =>
                {
                    var mob = Instantiate(_prefab);
                    mob.onDespawn += OnMobDespawn;
                    mob.onDeath += OnMobDeath;
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
                },
                o =>
                {
                    o.onDespawn -= OnMobDespawn;
                    o.onDeath -= OnMobDeath;
                });
        }

        private void OnMobDespawn(MobController mob)
        {
            _pool.Release(mob);
            onMobDespawn?.Invoke();
        }

        private void OnMobDeath(MobController mob)
        {
            _pool.Release(mob);
            onMobDeath?.Invoke();
        }

        public void Spawn()
        {
            var mob = _pool.Get();
            var lifetime = (GameFieldInfo.Top2BottomDistance + YTopOffset - YBottomOffset) / mob.FallSpeed;
            var randomWorldPos = new Vector3(Random.Range(GameFieldInfo.LeftBound, GameFieldInfo.RightBound),
                GameFieldInfo.TopBound + YTopOffset, 0);
            var cellPosition = _grid.WorldToCell(randomWorldPos);
            mob.transform.position = _grid.GetCellCenterWorld(cellPosition);
            mob.Launch(lifetime);
        }

        public void Clear()
        {
            var activeCount = _activeMobs.Count;
            for (var i = 0; i < activeCount; i++)
            {
                var mob = _activeMobs[0];
                _pool.Release(mob);
            }
        }
    }
}