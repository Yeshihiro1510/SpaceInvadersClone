using UnityEngine;
using UnityEngine.InputSystem;

namespace Source
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField, Min(0)] private float _absoluteBounds;

        private InputSystem_Actions _inputSystem;

        private float _xInput;

        private void Awake()
        {
            _inputSystem = new InputSystem_Actions();
            _inputSystem.Player.Move.performed += OnMove;
            _inputSystem.Player.Move.canceled += OnStop;
            _inputSystem.Player.Enable();
        }

        private void Update()
        {
            var delta = _xInput * _speed * Time.deltaTime;
            if (CheckBounds(delta)) transform.Translate(new Vector2(delta, 0));
        }

        private void OnDestroy()
        {
            _inputSystem.Player.Move.performed -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context) => _xInput = context.ReadValue<Vector2>().x;
        private void OnStop(InputAction.CallbackContext _) => _xInput = 0;
        
        private bool CheckBounds(float delta)
        {
            var deltaPosition = transform.position.x + delta;
            return deltaPosition >= -_absoluteBounds && deltaPosition <= _absoluteBounds;
        }
    }
}