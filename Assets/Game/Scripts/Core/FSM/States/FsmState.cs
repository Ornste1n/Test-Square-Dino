namespace Game.Scripts.Core.FSM.States
{
    public abstract class FsmState<T> where T : FsmState<T>
    {
        protected readonly Fsm<T> Fsm;

        public FsmState(Fsm<T> fsm) => Fsm = fsm;
        
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
