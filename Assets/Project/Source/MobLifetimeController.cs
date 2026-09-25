using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Project.Source
{
    public class MobLifetimeController : MonoBehaviour
    {
        [field: SerializeField] public float SpawnRate { get; private set; }
        [field: SerializeField] public float YSpawnOffset { get; private set; }
        [field: SerializeField] public float YDespawnOffset { get; private set; }

        [SerializeField] private MobController _prefab;

        private ObjectPool<MobController> _pool;
        private readonly List<MobController> _activeMobs = new();
        private float _lifetimeWayDistance;

        private float _spawnTimer;

        private void Awake()
        {
            _pool = new ObjectPool<MobController>(() => Instantiate(_prefab),
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

        private void Start()
        {
            _lifetimeWayDistance = GameFieldInfo.TopBound - (GameFieldInfo.BottomBound + YDespawnOffset);
        }

        private void Update()
        {
            _spawnTimer += Time.deltaTime;
            
            if (_spawnTimer >= SpawnRate)
            {
                var mob = _pool.Get();
                mob.transform.position = new Vector3(Random.Range(GameFieldInfo.LeftBound, GameFieldInfo.RightBound), GameFieldInfo.TopBound, 0);
                StartCoroutine(MobRoutine(mob));
                _spawnTimer = 0;
            }
        }

        private IEnumerator MobRoutine(MobController target)
        {
            yield return new WaitForSeconds(_lifetimeWayDistance / target.FallSpeed);
            _pool.Release(target);
        }
    }
}