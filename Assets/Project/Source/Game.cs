using UnityEngine;

namespace Project.Source
{
    public class Game : MonoBehaviour
    {
        [field: SerializeField] public float MobsSpawnRate { get; private set; }
        
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private MobLifetimeController _mobLifetime;
        [SerializeField] private PointsController _points;
        
        private float _mobsSpawnTimer;

        private void Awake()
        {
            var player = Instantiate(_playerPrefab);
            player.transform.position = new Vector3(0, GameFieldInfo.BottomBound, 0);
        }

        private void Update()
        {
            _mobsSpawnTimer += Time.deltaTime;

            if (_mobsSpawnTimer >= MobsSpawnRate)
            {
                var mob = _mobLifetime.Spawn();
                mob.onDeath += _points.AddPoints;
                mob.onDespawn += OnMobDespawn;
                _mobsSpawnTimer = 0;
            }
        }
        
        private void OnMobDespawn(MobController mob)
        {
            mob.onDeath -= _points.AddPoints;
            mob.onDespawn -= OnMobDespawn;
        }
    }
}