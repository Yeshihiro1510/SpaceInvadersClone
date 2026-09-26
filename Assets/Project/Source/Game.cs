using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class Game : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float MobsSpawnRate { get; private set; }
        [field: SerializeField, Min(0)] public int MobsCount { get; private set; }
        [field: SerializeField, Min(0)] public int RewardPerMob { get; private set; }

        [SerializeField] private PlayerController _player;
        [SerializeField] private MobLifetimeController _mobLifetime;
        [SerializeField] private DialogField _dialogField;
        [SerializeField] private TMP_Text _pointsText;
        [SerializeField] private BackgroundFx _backgroundFx;
        private Points _points;

        private float _mobsSpawnTimer;
        private int _mobsLeft;
        private bool _isGameOver;

        private void Awake()
        {
            _points = new Points(_pointsText);

            GlobalServices.InputSystem.Player.Reload.performed += OnReload;

            _player.onDeath += Reload;
            _mobLifetime.onMobDeath += OnMobDeath;
            _mobLifetime.onMobDespawn += OnMobDespawn;

            _mobsLeft = MobsCount;
        }
        
        private void Start()
        {
            _backgroundFx.StartScrolling();
        }

        private void Update()
        {
            if (_isGameOver) return;

            _mobsSpawnTimer += Time.deltaTime;

            if (_mobsLeft > 0)
            {
                if (_mobsSpawnTimer >= MobsSpawnRate)
                {
                    _mobLifetime.Spawn();
                    _mobsSpawnTimer = 0;
                    _mobsLeft--;
                }
            }
            else if (_mobLifetime.ActiveMobCount <= 0)
            {
                _isGameOver = true;
                _backgroundFx.StopScrolling();
                _dialogField.DOText($"Congratulations! You've completed prototype with {_points.Value} points!\nPress [R] to restart the game . . .");
            }
        }

        private void OnDestroy()
        {
            GlobalServices.InputSystem.Player.Reload.performed -= OnReload;

            _mobLifetime.onMobDeath -= OnMobDeath;
            _mobLifetime.onMobDespawn -= OnMobDespawn;
        }

        private void OnReload(InputAction.CallbackContext _) => Reload();
        private void OnMobDespawn() => _player.TakeDamage(1f);
        private void OnMobDeath() => _points.Value += RewardPerMob;

        private void Reload()
        {
            _mobLifetime.Clear();
            _points.Value = 0;

            _mobsSpawnTimer = 0;
            _mobsLeft = MobsCount;

            if (_isGameOver) _backgroundFx.StartScrolling();
            _dialogField.DOText("");
            _player.Respawn();
            
            _isGameOver = false;
        }
    }
}