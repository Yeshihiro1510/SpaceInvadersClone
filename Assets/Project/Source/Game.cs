using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Project.Source
{
    public class Game : MonoBehaviour
    {
        [field: SerializeField] public float MobsSpawnRate { get; private set; }
        [field: SerializeField] public int MobsCount { get; private set; }

        [SerializeField] private PlayerController _player;
        [SerializeField] private MobLifetimeController _mobLifetime;
        [SerializeField] private PointsController _points;
        [SerializeField] private Text _congratulationsText;

        private float _mobsSpawnTimer;
        private int _mobsLeft;
        private bool _isGameOver;

        private void Awake()
        {
            GlobalInputSystem.InputSystem.Player.Reload.performed += OnReload;

            _player.onDeath += Reload;

            _mobLifetime.onMobDeath += _points.AddPoints;
            _mobLifetime.onMobDespawn += OnMobDespawn;

            _mobsLeft = MobsCount;
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
                _congratulationsText.DOText(
                    $"Congratulations! You've earned {_points.Value} points!\nPress [R] to restart the game . . .",
                    1.5f, false);
            }
        }

        private void OnDestroy()
        {
            GlobalInputSystem.InputSystem.Player.Reload.performed -= OnReload;

            _mobLifetime.onMobDeath -= _points.AddPoints;
            _mobLifetime.onMobDespawn -= OnMobDespawn;
        }

        private void OnReload(InputAction.CallbackContext _) => Reload();
        private void OnMobDespawn() => _player.TakeDamage(1f);

        private void Reload()
        {
            _mobLifetime.Clear();
            _points.ResetPoints();
            _player.Respawn();

            _mobsSpawnTimer = 0;
            _mobsLeft = MobsCount;
            _isGameOver = false;

            _congratulationsText.DOText("", 1.5f, false);
        }
    }
}