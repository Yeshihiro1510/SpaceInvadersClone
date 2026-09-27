using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [field: SerializeField, Min(0)] public float Speed { get; private set; }
        [field: SerializeField, Min(0)] public float AttackRate { get; private set; }
        [field: SerializeField, Min(1)] public int StarterHealth { get; private set; }
        
        private float _xInput;
        private bool _isAttacking;
        private float _attackingTimer;
        private int _health;

        public int Health
        {
            get => _health;
            set
            {
                if (value <= 0)
                {
                    _health = 0;
                    onDeath?.Invoke();
                }
                else _health = value;
                
                UI.HealthField.SetHealth(value);
            }
        }

        public event Action onDeath;

        private void Awake()
        {
            GlobalServices.InputSystem.Player.Move.performed += OnMove;
            GlobalServices.InputSystem.Player.Move.canceled += OnStop;
            GlobalServices.InputSystem.Player.Attack.performed += OnAttack;
            GlobalServices.InputSystem.Player.Attack.canceled += OnStopAttacking;
            Respawn();
        }

        private void OnEnable() => GlobalServices.InputSystem.Player.Enable();
        private void OnDisable() => GlobalServices.InputSystem.Player.Disable();

        private void Update()
        {
            _attackingTimer += Time.deltaTime;

            var delta = _xInput * Speed * Time.deltaTime;

            if (CheckBounds(delta)) transform.Translate(new Vector2(delta, 0));
            if (_isAttacking && _attackingTimer >= AttackRate)
            {
                var bullet1 = G.BulletFactory.CreatePlayerBullet(Vector2.up);
                var bullet2 = G.BulletFactory.CreatePlayerBullet(Vector2.up);
                bullet1.transform.position = transform.position + new Vector3(0.2f, 1f, 0);
                bullet2.transform.position = transform.position + new Vector3(-0.2f, 1f, 0);
                _attackingTimer = 0;
            }
        }

        private void OnDestroy()
        {
            GlobalServices.InputSystem.Player.Move.performed -= OnMove;
            GlobalServices.InputSystem.Player.Move.canceled -= OnStop;
            GlobalServices.InputSystem.Player.Attack.performed -= OnAttack;
            GlobalServices.InputSystem.Player.Attack.canceled -= OnStopAttacking;
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

        public void TakeDamage(int damage) => Health -= damage;
        public void Respawn() => Health = StarterHealth;
    }
}