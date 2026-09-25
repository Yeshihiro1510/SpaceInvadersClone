using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float Speed { get; private set; }
        [field: SerializeField, Min(0)] public float AttackRate { get; private set; }

        private BulletLifetimeController _bulletLifetime;
        private InputSystem_Actions _inputSystem;

        private float _xInput;
        private bool _isAttacking;
        private float _attackingTimer;

        private void Awake()
        {
            _inputSystem = new InputSystem_Actions();
            _inputSystem.Player.Move.performed += OnMove;
            _inputSystem.Player.Move.canceled += OnStop;
            _inputSystem.Player.Attack.performed += OnAttack;
            _inputSystem.Player.Attack.canceled += OnStopAttacking;

            _bulletLifetime = FindAnyObjectByType<BulletLifetimeController>();
        }

        private void OnEnable()
        {
            _inputSystem.Player.Enable();
        }

        private void OnDisable()
        {
            _inputSystem.Player.Disable();
        }

        private void Update()
        {
            _attackingTimer += Time.deltaTime;

            var delta = _xInput * Speed * Time.deltaTime;

            if (CheckBounds(delta)) transform.Translate(new Vector2(delta, 0));
            if (_isAttacking && _attackingTimer >= AttackRate)
            {
                
                var bullet = _bulletLifetime.Spawn();
                bullet.transform.position = transform.position;
                _attackingTimer = 0;
            }
        }

        private void OnDestroy()
        {
            _inputSystem.Player.Move.performed -= OnMove;
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
    }
}