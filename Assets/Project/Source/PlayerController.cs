using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Source
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public float Speed { get; private set; }

        private InputSystem_Actions _inputSystem;

        private float _xInput;
        
        private void Awake()
        {
            _inputSystem = new InputSystem_Actions();
            _inputSystem.Player.Move.performed += OnMove;
            _inputSystem.Player.Move.canceled += OnStop;
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
            var delta = _xInput * Speed * Time.deltaTime;
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
            return deltaPosition >= GameFieldInfo.LeftBound && deltaPosition <= GameFieldInfo.RightBound;
        }
    }
}