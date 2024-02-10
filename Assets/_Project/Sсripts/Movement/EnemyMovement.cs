using System;
using System.Collections.Generic;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Scriptable;
using UnityEngine;

namespace _Project.Sсripts.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        private Cell _targetCell;
        private Dictionary<EffectName, Effect> _enemyEffects;
        private EnemyAimToCellMover _aimMover;

        public void Initialize(Cell targetCell, Dictionary<EffectName, Effect> enemyEffects, EnemyAimToCellMover aimMover)
        {
            _targetCell = targetCell;
            _enemyEffects = enemyEffects;
            _aimMover = aimMover;

        }

        public event Action TurnCompleted;

        public void Move()
        {
            EffectName effectName = _targetCell.EffectName;
            _enemyEffects[effectName].Activate(() =>
            {
                _aimMover.SetToNewRandomCell();
                TurnCompleted?.Invoke();
            });
        }
    }
}