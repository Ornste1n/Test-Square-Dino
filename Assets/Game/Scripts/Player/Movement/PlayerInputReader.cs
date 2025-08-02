using UnityEngine;
using Game.Scripts.Input;

namespace Game.Scripts.Player.Movement
{
    public class PlayerInputReader : MonoBehaviour
    {
        private Controls _controls;
        
        public Vector2 MoveInput { get; private set; }

        private void OnEnable()
        {
            _controls = new Controls();
            _controls.Enable();
        }

        private void Update()
        {
            MoveInput = _controls.Player.Movement.ReadValue<Vector2>();
        }
        
        private void OnDisable() => _controls.Disable();
        private void OnDestroy() => _controls.Dispose();
    }
}