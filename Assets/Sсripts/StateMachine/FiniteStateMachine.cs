using System;
using System.Collections.Generic;

namespace Sсripts.StateMachine
{
    public class FiniteStateMachine
    {
        // == наброски по стейтам == 
        // Initialize
        // PlayerTurn (возможно стоит разделить на применение заклинания и всё остальное)
        // EnemyDefeat (спаун нового врага)
        // EnemyTurn
        // PlayerDefeat (посмотреть рекламу за воскрешение)
        // EndOfGame
        
        private FsmState _currentFsmState;

        private Dictionary<Type, FsmState> _states = new();

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
            }
        }
    }
}