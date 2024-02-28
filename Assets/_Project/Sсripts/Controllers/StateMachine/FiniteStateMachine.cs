using System;
using System.Collections.Generic;

namespace _Project.Sсripts.Controllers.StateMachine
{
    public class FiniteStateMachine
    {
        private FsmState _currentFsmState;

        private readonly Dictionary<Type, FsmState> _states = new();

        public event Action StateChanged;

        public void AddState(FsmState fsmState)
        {
            _states.Add(fsmState.GetType(), fsmState);
        }

        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);

            if (_currentFsmState != null && _currentFsmState.GetType() == type)
                return;

            if (_states.TryGetValue(type, out var newState))
            {
                _currentFsmState?.Exit();
                _currentFsmState = newState;
                _currentFsmState.Enter();
                StateChanged?.Invoke();
            }
        }
    }
}