using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Mechanics
{
    public class CubeSpawner : NetworkBehaviour
    {
        [SerializeField] private InputAction _spawnAction;
        [Space]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _spawnDistance = 2f;
        
        public override void OnStartLocalPlayer()
        {
            _spawnAction.performed += OnPerformed;
            _spawnAction.Enable();
        }
        
        private void OnPerformed(InputAction.CallbackContext _)
        {
            CmdSpawnCube();
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnCube()
        {
            Vector3 position = _playerTransform.position + _playerTransform.forward * _spawnDistance;
            position.y += _spawnDistance;
            GameObject cube = Instantiate(_prefab, position, Quaternion.identity);
            NetworkServer.Spawn(cube);
        }

        public override void OnStopLocalPlayer()
        {
            _spawnAction.performed -= OnPerformed;
            _spawnAction.Enable();
        }
    }
}