using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Movement;

namespace _Project.Sсripts.Services
{
    public class LevelRestarter
    {
        private readonly CellsManager _cellsManager;

        private readonly Defence _playerDefence;
        private readonly Health _playerHealth;
        private readonly Damage _playerDamage;
        private readonly Mana _playerMana;

        private readonly Defence _enemyDefence;
        private readonly Health _enemyHealth;
        private readonly Damage _enemyDamage;
        private readonly EnemyLevel _enemyLevel;

        private readonly AvailableSpells _availableSpells;
        private readonly PlayerMovement _playerMovement;
        private readonly EnemyTargetController _enemyTargetController;

        public LevelRestarter(CellsManager cellsManager, Defence playerDefence, Health playerHealth,
            Damage playerDamage, Mana playerMana, Defence enemyDefence, Health enemyHealth, Damage enemyDamage,
            AvailableSpells availableSpells, PlayerMovement playerMovement, EnemyTargetController enemyTargetController,
            EnemyLevel enemyLevel)
        {
            _cellsManager = cellsManager;
            _playerDefence = playerDefence;
            _playerHealth = playerHealth;
            _playerDamage = playerDamage;
            _playerMana = playerMana;
            _enemyDefence = enemyDefence;
            _enemyHealth = enemyHealth;
            _enemyDamage = enemyDamage;
            _availableSpells = availableSpells;
            _playerMovement = playerMovement;
            _enemyTargetController = enemyTargetController;
            _enemyLevel = enemyLevel;
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

            // заменить модель противника, пока просто сделать крупнее - плавно 
            // эффект на модель противника 
            
            _enemyLevel.Increase();
            _enemyDefence.Increase(1);
            _enemyDamage.Increase(1);
            _enemyHealth.IncreaseMaxValue(2);
            _enemyHealth.ReplenishToMax();
        }
    }
}