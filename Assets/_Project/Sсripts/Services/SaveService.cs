using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Domain.Spells;
using Agava.YandexGames;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Sсripts.Services
{
    // TODO: как пороверить это всё:
    //    + кешировать локально json
    //    - загрузить билд в ЯИ (но непонятно как блок с #if отработает)

    public class SaveService
    {
        private string _localSaveJson;

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

        public SaveService(Damage playerDamage, Defence playerDefence, Health playerHealth, Mana playerMana,
            AvailableSpells availableSpells, PlayerMovement playerMovement, EnemyLevel enemyLevel, Damage enemyDamage,
            Defence enemyDefence, Health enemyHealth, EnemyTargetController enemyTargetController,
            CellsManager cellsManager, FiniteStateMachine finiteStateMachine)
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
                CellIndexesWithEffectNames = _cellsManager.GetCellIndexesWithEffectNames(),
                FsmStateTypeName = _finiteStateMachine.GetCurrentStateTypeName()
            };

            _localSaveJson = JsonConvert.SerializeObject(dataRecord);

#if UNITY_WEBGL && !UNITY_EDITOR
            CloudSave();
#endif
        }

        public void Load()
        {
            
#if UNITY_EDITOR
            LocalLoad();
            LoadComplete = true;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            CloudLoad();
#endif
            
            
        }

        private void CloudLoad()
        {
            Debug.LogError("CloudLoad - STARTED");
            PlayerAccount.GetCloudSaveData((data) =>
                {
                    Handle(data);
                    Debug.LogError("PlayerAccount.GetCloudSaveData - COMPLETED");
                    LoadComplete = true;
                },
                errorMessage =>
                {
                    Debug.LogError($"PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
                    LoadComplete = true;
                });
        }

        private void LocalLoad()
        {
            Handle(_localSaveJson);
        }

        private void CloudSave()
        {
            Debug.LogError("CloudSave - STARTED");
            PlayerAccount.SetCloudSaveData(_localSaveJson, () => Debug.LogError("PlayerAccount.SetCloudSaveData - SUCCESS"));
        }

        private void Handle(string data)
        {
            if (IsExisted(data) == false)
                return;

            Apply(Deserialize(data));
        }

        private bool IsExisted(string data)
        {
            IsSaveExist = string.IsNullOrEmpty(data) == false;
            return IsSaveExist;
        }

        private DataRecord Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<DataRecord>(data);
        }

        private void Apply(DataRecord dataRecord)
        {
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
            _finiteStateMachine.SetState(Type.GetType(dataRecord.FsmStateTypeName));

            LoadComplete = true;
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
        }
    }
}