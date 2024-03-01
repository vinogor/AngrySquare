using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Sсripts.Controllers.StateMachine
{
    public class FiniteStateMachine
    {
        private FsmState _currentFsmState;

        private readonly Dictionary<Type, FsmState> _states = new();

        public event Action StateChanged;

        public string GetCurrentStateTypeName() => _currentFsmState.GetType().FullName;

        public void AddState(FsmState fsmState)
        {
            _states.Add(fsmState.GetType(), fsmState);
        }

        public void SetState<T>() where T : FsmState
        {
            var type = typeof(T);
            SetState(type);
        }

        public void SetState(Type type)
        {
            if (_currentFsmState != null && _currentFsmState.GetType() == type)
            {
                Debug.Log("FiniteStateMachine SetState - cant set state - type = " + type);
                return;
            }
                

            if (_states.TryGetValue(type, out var newState))
            {
                _currentFsmState?.Exit();
                _currentFsmState = newState;
                _currentFsmState.Enter();
                StateChanged?.Invoke();
                Debug.Log("FiniteStateMachine SetState - state  set - " + newState);
            }
        }
    }
}