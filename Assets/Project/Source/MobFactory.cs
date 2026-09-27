using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Source
{
    public class MobFactory
    {
        private readonly MobController _prefab = Resources.Load<MobController>("Mob");
        
        private readonly ObjectPool<MobController> _pool;
        private readonly List<MobController> _activeMobs = new();
        private readonly Grid _grid;
        
        public int ActiveMobCount => _activeMobs.Count;
        public MobController[] ActiveMobs => _activeMobs.ToArray();

        public event Action onMobDeath;
        public event Action onMobDespawn;

        public MobFactory()
        {
            _pool = new ObjectPool<MobController>(() =>
                {
                    var mob = Object.Instantiate(_prefab);
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
            
            _grid = Object.FindAnyObjectByType<Grid>();
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

        public void Create()
        {
            var mob = _pool.Get();
            var lifetime = (GameFieldInfo.Top2BottomDistance + 2) / mob.FallSpeed;
            var randomWorldPos = new Vector3(Random.Range(GameFieldInfo.LeftBound, GameFieldInfo.RightBound), GameFieldInfo.TopBound + 1, 0);
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