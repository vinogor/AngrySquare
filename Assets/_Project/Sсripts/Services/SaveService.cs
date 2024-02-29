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

namespace _Project.Sсripts.Services
{
    // TODO: добавить стейт Инициализация + перетащить туда из композита что надо 
    // TODO: авторизация - как чё?
    // TODO: а вызов загрузки как будет? 
    // TODO: ??? как пороверить это всё?
    //    + кешировать локально json - проверить +
    //    - загрузить билд в ЯИ

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

            // PlayerAccount.SetCloudSaveData(json, () => Debug.Log("PlayerAccount.SetCloudSaveData - SUCCESS"));
        }

        public void Load()
        {
            // PlayerAccount.GetCloudSaveData((data) =>
            // {
            // IsSaveExist = string.IsNullOrEmpty(data) == false;
            // DataRecord dataRecord = JsonConvert.DeserializeObject<DataRecord>(data);

            IsSaveExist = string.IsNullOrEmpty(_localSaveJson) == false;

            if (IsSaveExist == false)
            {
                LoadComplete = true;
                return;
            }

            DataRecord dataRecord = JsonConvert.DeserializeObject<DataRecord>(_localSaveJson);

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
            _finiteStateMachine.SetState(Type.GetType(dataRecord.FsmStateTypeName)); // может плохо работать в браузере

            LoadComplete = true;

            // Debug.Log("PlayerAccount.GetCloudSaveData - SUCCESS");
            // }, (errorMessage) =>
            // {
            // LoadComplete = true;
            // Debug.Log($"PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
            // });
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