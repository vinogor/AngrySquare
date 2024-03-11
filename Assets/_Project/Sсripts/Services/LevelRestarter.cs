using Config;
using Controllers;
using Controllers.PopupChoice;
using Domain;
using Domain.Movement;
using Domain.Spells;

namespace Services
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
        private readonly EnemyMovement _enemyMovement;

        private readonly AvailableSpells _availableSpells;
        private readonly PlayerMovement _playerMovement;
        private readonly EnemyTargetController _enemyTargetController;

        private readonly PopUpChoiceEffectPresenter _popUpChoiceEffectPresenter;
        private readonly PopUpChoiceSpellPresenter _popUpChoiceSpellPresenter;
        private readonly DiceRoller _diceRoller;

        public LevelRestarter(CellsManager cellsManager, Defence playerDefence, Health playerHealth,
            Damage playerDamage, Mana playerMana, EnemyProgression enemyProgression, Defence enemyDefence,
            Health enemyHealth, Damage enemyDamage, AvailableSpells availableSpells, PlayerMovement playerMovement,
            EnemyTargetController enemyTargetController, EnemyLevel enemyLevel, EnemyMovement enemyMovement,
            PopUpChoiceEffectPresenter popUpChoiceEffectPresenter, PopUpChoiceSpellPresenter popUpChoiceSpellPresenter,
            DiceRoller diceRoller
        )
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
            _enemyMovement = enemyMovement;

            _availableSpells = availableSpells;
            _playerMovement = playerMovement;
            _enemyTargetController = enemyTargetController;

            _popUpChoiceEffectPresenter = popUpChoiceEffectPresenter;
            _popUpChoiceSpellPresenter = popUpChoiceSpellPresenter;
            _diceRoller = diceRoller;
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

            _playerMovement.ForceStop();
            _playerMovement.SetDefaultStayCell();

            _enemyDefence.SetToDefault();
            _enemyHealth.SetToDefault();
            _enemyDamage.SetToDefault();
            _enemyLevel.SetToDefault();
            _enemyMovement.ForceStop();
            _enemyMovement.SetDefaultPosition();

            _enemyTargetController.SetAimToNewRandomTargetCell();

            _popUpChoiceEffectPresenter.HidePopup();
            _popUpChoiceSpellPresenter.HidePopup();
            _diceRoller.MakeUnavailable();
            _diceRoller.SetToBasePosition();
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