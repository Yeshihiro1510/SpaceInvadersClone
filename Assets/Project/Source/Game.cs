using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class Game : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float MobsSpawnRate { get; private set; }
        [field: SerializeField, Min(0)] public int MobsCount { get; private set; }
        [field: SerializeField, Min(0)] public int RewardPerMob { get; private set; }
        [field: SerializeField, Min(0)] public float MobsShootingRate { get; private set; }

        [SerializeField] private BackgroundFx _backgroundFx;
        private PlayerController _player;
        private Points _points;

        private float _mobsSpawnTimer;
        private float _mobsShootTimer;
        private int _mobsLeft;
        private bool _isGameOver;

        private void Awake()
        {
            G.BulletFactory = new BulletFactory();
            G.MobFactory = new MobFactory();
            
            _points = new Points(UI.Instance.PointsText);
            _player = Instantiate(Resources.Load<PlayerController>("Player"));
            _player.transform.position = GameFieldInfo.Center + new Vector2(0, GameFieldInfo.BottomBound) * 0.7f;

            GlobalServices.InputSystem.Player.Reload.performed += OnReload;

            _player.onDeath += OnPlayerDeath;
            G.MobFactory.onMobDeath += OnMobDeath;
            G.MobFactory.onMobDespawn += OnMobDespawn;

            _mobsLeft = MobsCount;
        }

        private void Start()
        {
            _backgroundFx.StartScrolling();
        }

        private void Update()
        {
            _backgroundFx.Tick(_player.transform.position.x);
            if (_isGameOver) return;

            _mobsSpawnTimer += Time.deltaTime;
            _mobsShootTimer += Time.deltaTime;

            if (_mobsLeft > 0)
            {
                if (_mobsSpawnTimer >= MobsSpawnRate)
                {
                    _mobsSpawnTimer = 0;
                    G.MobFactory.Create();
                    _mobsLeft--;
                }

                if (G.MobFactory.ActiveMobCount > 0 && _mobsShootTimer >= MobsShootingRate)
                {
                    _mobsShootTimer = 0;
                    var bullet = G.BulletFactory.CreateMobBullet(Vector2.down);
                    bullet.transform.position = G.MobFactory.ActiveMobs[Random.Range(0, G.MobFactory.ActiveMobCount)]
                        .transform.position + Vector3.down;
                }
            }
            else if (G.MobFactory.ActiveMobCount <= 0)
            {
                _isGameOver = true;
                _backgroundFx.StopScrolling();
                UI.Instance.DialogField.DOText($"Congratulations! You've completed prototype with {_points.Value} points!\nPress [R] to restart the game . . .");
            }
        }

        private void OnDestroy()
        {
            GlobalServices.InputSystem.Player.Reload.performed -= OnReload;

            _player.onDeath -= OnPlayerDeath;
            G.MobFactory.onMobDeath -= OnMobDeath;
            G.MobFactory.onMobDespawn -= OnMobDespawn;
        }

        private void OnReload(InputAction.CallbackContext _) => Reload();
        private void OnMobDespawn() => _player.TakeDamage(1);
        private void OnMobDeath() => _points.Value += RewardPerMob;

        private void OnPlayerDeath()
        {
            _isGameOver = true;
            G.MobFactory.Clear();
            _backgroundFx.StopScrolling();
            UI.Instance.DialogField.DOText($"Its total fail! You ended up with {_points.Value} points.\nPress [R] to restart the game . . .");
        }

        private void Reload()
        {
            G.MobFactory.Clear();
            
            _points.Value = 0;
            _mobsSpawnTimer = 0;
            _mobsLeft = MobsCount;

            if (_isGameOver) _backgroundFx.StartScrolling();
            UI.Instance.DialogField.DOText("");
            _player.Respawn();

            _isGameOver = false;
        }
    }
}