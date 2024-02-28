using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.Domain.Spells;
using Agava.YandexGames;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine;

namespace _Project.Sсripts.Services
{
    // TODO: добавить стейт Инициализация + перетащить туда из композита что надо 
    // TODO: авторизация - как чё?
    // TODO: а вызов загрузки как будет? 
    // TODO: ??? сериализация листов и енамов?   public Dictionary<int, EffectName>
    // TODO: + фаза стейт машины - сохранить как какой-то енам? 
    // TODO: пришлось в модельных классах открыть setter для value  
    // TODO: ??? как пороверить это всё? Кнопка в инспекторе 

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
        // TODO: + фазу стейт машины

        public SaveService(Damage playerDamage, Defence playerDefence, Health playerHealth, Mana playerMana,
            AvailableSpells availableSpells, PlayerMovement playerMovement, EnemyLevel enemyLevel, Damage enemyDamage,
            Defence enemyDefence, Health enemyHealth, EnemyTargetController enemyTargetController,
            CellsManager cellsManager)
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
            // TODO: + вид модели 

            _cellsManager = cellsManager;
            // TODO: + фазу стейт машины
        }

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
                SpellNames = _availableSpells.SpellNames, // не сериализовалось
                PlayersCellIndex = _playerMovement.PlayersCellIndex,

                // enemy
                EnemyLevelValue = _enemyLevel.Value,
                EnemyDamageValue = _enemyDamage.Value,
                EnemyDefenceValue = _enemyDefence.Value,
                EnemyHealthValue = _enemyHealth.Value,
                EnemyHealthMaxValue = _enemyHealth.MaxValue,
                TargetCellsIndexes = _enemyTargetController.GetCurrentTargetCells() // не сериализовалось
                    .Select(cell => _cellsManager.Index(cell)).ToList(),
                // TODO: + вид модели 

                // common
                CellIndexesWithEffectNames = _cellsManager.GetCellIndexesWithEffectNames()
                // TODO: + фазу стейт машины
            };

            string jsonString = JsonSerializer.ToJsonString(dataRecord);
            string json = JsonUtility.ToJson(dataRecord);

            PlayerAccount.SetCloudSaveData(json, () => Debug.Log("PlayerAccount.SetCloudSaveData - SUCCESS"));
        }

        public void Load()
        {
            PlayerAccount.GetCloudSaveData((data) =>
            {
                DataRecord dataRecord = JsonUtility.FromJson<DataRecord>(data);

                _playerDamage.Value = dataRecord.PlayerDamageValue;
                _playerDefence.Value = dataRecord.PlayerDefenceValue;
                _playerHealth.Value = dataRecord.PlayerHealthValue;
                _playerHealth.MaxValue = dataRecord.PlayerHealthMaxValue;
                _playerMana.Value = dataRecord.PlayerManaValue;
                _playerMana.MaxValue = dataRecord.PlayerManaMaxValue;
                _availableSpells.Clear();
                dataRecord.SpellNames.ForEach(_availableSpells.Add);
                _playerMovement.SetNewStayCell(dataRecord.PlayersCellIndex);

                _enemyLevel.Value = dataRecord.EnemyLevelValue;
                _enemyDamage.Value = dataRecord.EnemyDamageValue;
                _enemyDefence.Value = dataRecord.EnemyDefenceValue;
                _enemyHealth.Value = dataRecord.EnemyHealthValue;
                _enemyHealth.MaxValue = dataRecord.EnemyHealthMaxValue;
                _enemyTargetController.SetNewTargetCells(dataRecord.TargetCellsIndexes);
                // TODO: + вид модели 

                _cellsManager.SetCellsEffects(dataRecord.CellIndexesWithEffectNames);
                // TODO: добавить фазу стейт машины

                Debug.Log("PlayerAccount.GetCloudSaveData - SUCCESS");
            }, (errorMessage) => Debug.Log($"PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}"));
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
            // TODO: + вид модели 

            // common
            public Dictionary<int, EffectName> CellIndexesWithEffectNames;
            // TODO: + фаза стейт машины - сохранить какой-то енам? 
        }
    }
}