using System;
using _Project.SDK.Leader;
using UnityEngine;

namespace _Project.Domain
{
    public class EnemyLevel
    {
        private readonly int _defaultValue;
        private readonly YandexLeaderBoard _yandexLeaderBoard;

        public EnemyLevel(YandexLeaderBoard yandexLeaderBoard)
        {
            _defaultValue = 1;
            Value = _defaultValue;

            _yandexLeaderBoard = yandexLeaderBoard;
        }

        public event Action Changed;

        public event Action SetDefault;

        public int Value { get; private set; }

        public void Increase()
        {
            Value++;
#if UNITY_WEBGL && !UNITY_EDITOR
            SetPlayer();
#endif
            Changed?.Invoke();
        }

        public void SetToDefault()
        {
            Value = _defaultValue;
            SetDefault?.Invoke();
        }

        public void SetNewValue(int value)
        {
            Value = value;
#if UNITY_WEBGL && !UNITY_EDITOR
            SetPlayer();
#endif
            Changed?.Invoke();
        }

        public async void SetPlayer()
        {
            Debug.Log("EnemyLevel - SetPlayer - start...");
            await _yandexLeaderBoard.SetPlayer(Value);
            Debug.Log("EnemyLevel - SetPlayer - finish!");
        }
    }
}