using System.Collections.Generic;
using Game.Scripts.Core.FSM.States;

namespace Game.Scripts.Core.FSM
{
    public class Fsm<T> where T : FsmState<T>
    {
        public T CurrentState { get; private set; }
        public T PreviousState { get; private set; }

        private readonly Dictionary<int, T> _states = new();

        private int _currentIndex;
        private int _previousIndex;

        public bool TryAddState(int index, T fsmState) => _states.TryAdd(index, fsmState);

        public void SetState(int index)
        {
            if(CurrentState != null && _currentIndex == index) return;
            
            if(!_states.TryGetValue(index, out T state)) return;

            _previousIndex = _currentIndex;
            _currentIndex = index;
            SetState(state);
        }

        public void SetPreviousState()
        {
            (_currentIndex, _previousIndex) = (_previousIndex, _currentIndex);
            SetState(PreviousState);
        }
        
        private void SetState(T state)
        {
            PreviousState = CurrentState;
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}