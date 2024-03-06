using _Project.Config;
using _Project.Controllers;
using _Project.Domain;
using _Project.Domain.Movement;
using _Project.Domain.Spells;

namespace _Project.Services
{
    public class LevelRestarter
    {
        private readonly CellsManager _cellsManager;

        private readonly Defence _playerDefence;
        private readonly Health _playerHealth;
        private readonly Damage _playerDamage;
        private readonly Mana _playerMana;

        private readonly EnemyProgression _enemyProgression;
        private readonly Defence _enemyDefence;
        private readonly Health _enemyHealth;
        private readonly Damage _enemyDamage;
        private readonly EnemyLevel _enemyLevel;

        private readonly AvailableSpells _availableSpells;
        private readonly PlayerMovement _playerMovement;
        private readonly EnemyTargetController _enemyTargetController;

        public LevelRestarter(CellsManager cellsManager, Defence playerDefence, Health playerHealth,
            Damage playerDamage, Mana playerMana, EnemyProgression enemyProgression, Defence enemyDefence,
            Health enemyHealth, Damage enemyDamage, AvailableSpells availableSpells, PlayerMovement playerMovement,
            EnemyTargetController enemyTargetController, EnemyLevel enemyLevel)
        {
            _cellsManager = cellsManager;

            _playerDefence = playerDefence;
            _playerHealth = playerHealth;
            _playerDamage = playerDamage;
            _playerMana = playerMana;

            _enemyProgression = enemyProgression;
            _enemyDefence = enemyDefence;
            _enemyHealth = enemyHealth;
            _enemyDamage = enemyDamage;
            _enemyLevel = enemyLevel;

            _availableSpells = availableSpells;
            _playerMovement = playerMovement;
            _enemyTargetController = enemyTargetController;
        }

        public void RestartAfterDefeat()
        {
            _cellsManager.CleanAll();
            _cellsManager.FillWithEffects();

            _playerDefence.SetToDefault();
            _playerHealth.SetToDefault();
            _playerDamage.SetToDefault();
            _playerMana.SetToDefault();
            _availableSpells.Clear();
            _availableSpells.Add(SpellName.UpDamage);
            _playerMovement.SetDefaultStayCell();

            // TODO: задать начальную модель противника  
            _enemyDefence.SetToDefault();
            _enemyHealth.SetToDefault();
            _enemyDamage.SetToDefault();
            _enemyLevel.SetToDefault();

            _enemyTargetController.SetAimToNewRandomTargetCell();
        }

        public void RestartAfterWin()
        {
            _cellsManager.CleanAll();
            _cellsManager.FillWithEffects();

            _playerMovement.SetDefaultStayCell();
            _playerHealth.ReplenishToMax();
            _playerMana.ReplenishToMax();

            _enemyLevel.Increase();
            int enemyLevelValue = _enemyLevel.Value;
            _enemyDefence.SetNewValue(_enemyProgression.GetDefence(enemyLevelValue));
            _enemyDamage.SetNewValue(_enemyProgression.GetDamage(enemyLevelValue));
            int newValue = _enemyProgression.GetHealth(enemyLevelValue);
            _enemyHealth.SetNewValues(newValue, newValue);

            _enemyHealth.ReplenishToMax();
        }
    }
}