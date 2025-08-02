using UnityEngine;
using Game.Scripts.Core.FSM;

namespace Game.Scripts.Player.Movement.States
{
    public class PlayerRunningState : PlayerState
    {
        public PlayerRunningState(Fsm<PlayerState> fsm, IPlayerController controller) : base(fsm, controller) { }

        public override void Enter() => Controller.Animator.SetBool(RunningKey, true);

        public override void FixedUpdate()
        {
            HandleMove();
        }

        private void HandleMove()
        {
            IPlayerController controller = Controller;
            Transform transform = controller.Transform;
            
            Vector2 moveInput = ClientInput;
            float speed = controller.MoveSpeed;
            Vector2 movement = new(moveInput.x * speed, moveInput.y * speed);
            Vector3 movementVector = new(movement.x, transform.position.y, movement.y);

            Quaternion target = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * controller.RotationSpeed);
            
            controller.Character.Move(movementVector * Time.fixedDeltaTime);
        }
        
        public override void Exit() => Controller.Animator.SetBool(RunningKey, false);
    }
}