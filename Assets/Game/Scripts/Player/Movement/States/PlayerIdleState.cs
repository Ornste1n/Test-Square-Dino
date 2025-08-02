using Game.Scripts.Core.FSM;

namespace Game.Scripts.Player.Movement.States
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(Fsm<PlayerState> fsm, IPlayerController controller) : base(fsm, controller) { }
    }
}