using Mirror;
using UnityEngine;
using Game.Scripts.Core.FSM;
using Game.Scripts.Player.Movement.States;

namespace Game.Scripts.Player.Movement
{
    public class NetworkPlayerController : NetworkBehaviour, IPlayerController
    {
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;

        [Header("Configuration")] 
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        private Fsm<PlayerState> _fsm;
        private Vector2 _latestInput = Vector2.zero;

        #region IPlayerController
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public Animator Animator => _animator;
        public Transform Transform => transform;
        public CharacterController Character => _characterController;
        #endregion

        private enum PlayerStates { Idle, Running }

        private void Start() => InitializeMachine();

        public override void OnStartClient()
        {
            if (!isServer)
                _characterController.enabled = false;
        }

        private void FixedUpdate()
        {
            if (!isServer) return;
            
            _fsm.CurrentState.FixedUpdate();
        }

        private void Update()
        {
            if(!isLocalPlayer) return;
            
            _latestInput = _inputReader.MoveInput;
            CmdSendInput(_latestInput);
        }

        private void InitializeMachine()
        {
            _fsm = new Fsm<PlayerState>();
            
            PlayerIdleState idleState = new (_fsm, this);
            PlayerRunningState runningState = new (_fsm, this);

            _fsm.TryAddState((int)PlayerStates.Idle, idleState);
            _fsm.TryAddState((int)PlayerStates.Running, runningState);
            
            _fsm.SetState((int)PlayerStates.Idle);
        }

        [Command(channel = Channels.Unreliable)]
        private void CmdSendInput(Vector2 input)
        {
            _fsm.CurrentState.ClientInput = input;
            
            if (input.sqrMagnitude > 0.01f)
                _fsm.SetState((int)PlayerStates.Running);
            else
                _fsm.SetState((int)PlayerStates.Idle);
        }
    }
}