using UnityEngine;
using Game.Scripts.Core.FSM;
using Game.Scripts.Core.FSM.States;

namespace Game.Scripts.Player.Movement.States
{
    public class PlayerState : FsmState<PlayerState>
    {
        protected const string RunningKey = "Running";
        
        public Vector3 ClientInput { get; set; }
        protected IPlayerController Controller { get; }

        protected PlayerState(Fsm<PlayerState> fsm, IPlayerController controller) : base(fsm)
        {
            Controller = controller;
        }
    }
}