using System;
using System.Collections.Generic;

namespace _Project.Sсripts.StateMachine
{
    public class FiniteStateMachine
    {
        // == наброски по стейтам ==

        // Initialize
        //    - инициализации ячеек и прочего
        //    - выбор ячейки для будущей атаки врагом
        // PlayerTurn (возможно стоит разделить на применение заклинания и всё остальное)
        //    - начало - разблокировка кубика 
        //    - конец - окончание эффекта от ячейки
        // EnemyDefeat (спаун нового врага)
        //    - проверка жив ли протиник
        // EnemyTurn
        //    - прыжок на отмеченную ячеку
        //    - активация её эффекта
        //    - выбор ячейки для прыжка в следующем раунде
        // PlayerDefeat (посмотреть рекламу за воскрешение)
        //    - проверка жив ли игрок 
        // EndOfGame
        //    - выход из игры

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