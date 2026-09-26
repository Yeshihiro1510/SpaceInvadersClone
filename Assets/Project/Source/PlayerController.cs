using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [field: SerializeField, Min(0)] public float Speed { get; private set; }
        [field: SerializeField, Min(0)] public float AttackRate { get; private set; }
        [field: SerializeField, Min(1)] public float PlayerStarterHealth { get; private set; }
        
        [SerializeField] private BulletController _bulletPrefab;

        private BulletLifetimeController _bulletLifetime;

        private float _xInput;
        private bool _isAttacking;
        private float _attackingTimer;
        private float _playerHealth;

        public event Action onDeath;

        private void Awake()
        {
            GlobalInputSystem.InputSystem.Player.Move.performed += OnMove;
            GlobalInputSystem.InputSystem.Player.Move.canceled += OnStop;
            GlobalInputSystem.InputSystem.Player.Attack.performed += OnAttack;
            GlobalInputSystem.InputSystem.Player.Attack.canceled += OnStopAttacking;

            _bulletLifetime = FindAnyObjectByType<BulletLifetimeController>();
        }

        private void OnEnable()
        {
            GlobalInputSystem.InputSystem.Player.Enable();
        }

        private void OnDisable()
        {
            GlobalInputSystem.InputSystem.Player.Disable();
        }

        private void Update()
        {
            _attackingTimer += Time.deltaTime;

            var delta = _xInput * Speed * Time.deltaTime;

            if (CheckBounds(delta)) transform.Translate(new Vector2(delta, 0));
            if (_isAttacking && _attackingTimer >= AttackRate)
            {
                var bullet1 = _bulletLifetime.Spawn(_bulletPrefab, Vector2.up);
                var bullet2 = _bulletLifetime.Spawn(_bulletPrefab, Vector2.up);
                bullet1.transform.position = transform.position + new Vector3(0.2f, 1f, 0);
                bullet2.transform.position = transform.position + new Vector3(-0.2f, 1f, 0);
                _attackingTimer = 0;
            }
        }

        private void OnDestroy()
        {
            GlobalInputSystem.InputSystem.Player.Move.performed -= OnMove;
            GlobalInputSystem.InputSystem.Player.Move.canceled -= OnStop;
            GlobalInputSystem.InputSystem.Player.Attack.performed -= OnAttack;
            GlobalInputSystem.InputSystem.Player.Attack.canceled -= OnStopAttacking;
        }

        private void OnMove(InputAction.CallbackContext context) => _xInput = context.ReadValue<Vector2>().x;
        private void OnStop(InputAction.CallbackContext _) => _xInput = 0;
        private void OnAttack(InputAction.CallbackContext _) => _isAttacking = true;
        private void OnStopAttacking(InputAction.CallbackContext _) => _isAttacking = false;

        private bool CheckBounds(float delta)
        {
            var deltaPosition = transform.position.x + delta;
            return deltaPosition >= GameFieldInfo.LeftBound && deltaPosition <= GameFieldInfo.RightBound;
        }

        public void TakeDamage(float damage)
        {
            _playerHealth--;
            if (_playerHealth <= 0)
            {
                onDeath?.Invoke();
            }
        }

        public void Respawn()
        {
            _playerHealth = PlayerStarterHealth;
        }
    }
}