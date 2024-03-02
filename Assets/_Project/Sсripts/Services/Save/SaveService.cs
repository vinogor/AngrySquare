using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Domain.Spells;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Sсripts.Services.Save
{
    public class SaveService
    {
        // player 
        private readonly Damage _playerDamage;
        private readonly Defence _playerDefence;
        private readonly Health _playerHealth;
        private readonly Mana _playerMana;

        private readonly AvailableSpells _availableSpells;
        private readonly PlayerMovement _playerMovement;

        // enemy
        private readonly EnemyLevel _enemyLevel;
        private readonly Damage _enemyDamage;
        private readonly Defence _enemyDefence;
        private readonly Health _enemyHealth;

        private readonly EnemyTargetController _enemyTargetController;
        // TODO: + вид модели 

        // common
        private readonly CellsManager _cellsManager;
        private readonly FiniteStateMachine _finiteStateMachine;
        private readonly PopUpTutorialController _popUpTutorialController;
        
        private readonly ISaver _saver;

        public SaveService(Damage playerDamage, Defence playerDefence, Health playerHealth, Mana playerMana,
            AvailableSpells availableSpells, PlayerMovement playerMovement, EnemyLevel enemyLevel, Damage enemyDamage,
            Defence enemyDefence, Health enemyHealth, EnemyTargetController enemyTargetController,
            CellsManager cellsManager, FiniteStateMachine finiteStateMachine,
            PopUpTutorialController popUpTutorialController, ISaver saver)
        {
            _playerDamage = playerDamage;
            _playerDefence = playerDefence;
            _playerHealth = playerHealth;
            _playerMana = playerMana;
            _availableSpells = availableSpells;
            _playerMovement = playerMovement;

            _enemyLevel = enemyLevel;
            _enemyDamage = enemyDamage;
            _enemyDefence = enemyDefence;
            _enemyHealth = enemyHealth;
            _enemyTargetController = enemyTargetController;

            _cellsManager = cellsManager;
            _finiteStateMachine = finiteStateMachine;
            _popUpTutorialController = popUpTutorialController;
            
            _saver = saver;
        }

        public bool LoadComplete { get; private set; }
        public bool IsSaveExist { get; private set; }

        public void Save()
        {
            DataRecord dataRecord = new DataRecord
            {
                // player 
                PlayerDamageValue = _playerDamage.Value,
                PlayerDefenceValue = _playerDefence.Value,
                PlayerHealthValue = _playerHealth.Value,
                PlayerHealthMaxValue = _playerHealth.MaxValue,
                PlayerManaValue = _playerMana.Value,
                PlayerManaMaxValue = _playerMana.MaxValue,
                SpellNames = _availableSpells.SpellNames,
                PlayersCellIndex = _playerMovement.PlayersCellIndex,

                // enemy
                EnemyLevelValue = _enemyLevel.Value,
                EnemyDamageValue = _enemyDamage.Value,
                EnemyDefenceValue = _enemyDefence.Value,
                EnemyHealthValue = _enemyHealth.Value,
                EnemyHealthMaxValue = _enemyHealth.MaxValue,
                TargetCellsIndexes = _enemyTargetController.GetCurrentTargetCells()
                    .Select(cell => _cellsManager.Index(cell)).ToList(),

                // common
                IsTutorialEnable = _popUpTutorialController.IsEnable,
                CellIndexesWithEffectNames = _cellsManager.GetCellIndexesWithEffectNames(),
                FsmStateTypeName = _finiteStateMachine.GetCurrentStateTypeName(),
            };

            string json = JsonConvert.SerializeObject(dataRecord);

            _saver.Write(json);
        }

        public void Load()
        {
            string json = _saver.Read().Result;
            Handle(json);
            LoadComplete = true;
        }

        private void Handle(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                IsSaveExist = false;
                Debug.Log("SaveService Handle - data empty");
                return;
            }

            DataRecord dataRecord;

            try
            {
                dataRecord = JsonConvert.DeserializeObject<DataRecord>(data);
            }
            catch (Exception e)
            {
                Debug.Log("SaveService Handle error DeserializeObject: " + e);
                dataRecord = null;
            }

            Debug.Log("SaveService Handle - DeserializeObject success");

            if (dataRecord == null)
            {
                Debug.Log("SaveService Handle - dataRecord is null");
                IsSaveExist = false;
                return;
            }

            Apply(dataRecord);
        }

        private void Apply(DataRecord dataRecord)
        {
            Debug.Log("SaveService Apply - start...");
            _playerDamage.SetNewValue(dataRecord.PlayerDamageValue);
            _playerDefence.SetNewValue(dataRecord.PlayerDefenceValue);
            _playerHealth.SetNewValues(dataRecord.PlayerHealthValue, dataRecord.PlayerHealthMaxValue);
            _playerMana.SetNewValues(dataRecord.PlayerManaValue, dataRecord.PlayerManaMaxValue);
            _availableSpells.Clear();
            dataRecord.SpellNames.ForEach(_availableSpells.Add);
            _playerMovement.SetNewStayCell(dataRecord.PlayersCellIndex);

            _enemyLevel.SetNewValue(dataRecord.EnemyLevelValue);
            _enemyDamage.SetNewValue(dataRecord.EnemyDamageValue);
            _enemyDefence.SetNewValue(dataRecord.EnemyDefenceValue);
            _enemyHealth.SetNewValues(dataRecord.EnemyHealthValue, dataRecord.EnemyHealthMaxValue);
            _enemyTargetController.SetNewTargetCells(dataRecord.TargetCellsIndexes);

            _cellsManager.SetCellsEffects(dataRecord.CellIndexesWithEffectNames);
            _popUpTutorialController.Switch(dataRecord.IsTutorialEnable);
            Type type = Type.GetType(dataRecord.FsmStateTypeName);
            Debug.Log("SaveService Apply - type = " + type);
            _finiteStateMachine.SetState(type);

            IsSaveExist = true;
            Debug.Log("SaveService Apply - finish! IsSaveExist = " + IsSaveExist);
        }

        private class DataRecord
        {
            // player 
            public int PlayerDamageValue;
            public int PlayerDefenceValue;
            public int PlayerHealthValue;
            public int PlayerHealthMaxValue;
            public int PlayerManaValue;
            public int PlayerManaMaxValue;
            public List<SpellName> SpellNames;
            public int PlayersCellIndex;

            // enemy
            public int EnemyLevelValue;
            public int EnemyDamageValue;
            public int EnemyDefenceValue;
            public int EnemyHealthValue;
            public int EnemyHealthMaxValue;
            public List<int> TargetCellsIndexes;

            // common
            public Dictionary<int, EffectName> CellIndexesWithEffectNames;
            public string FsmStateTypeName;
            public bool IsTutorialEnable;
        }
    }
}