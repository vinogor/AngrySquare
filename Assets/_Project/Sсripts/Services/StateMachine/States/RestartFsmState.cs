using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Movement;

namespace _Project.Sсripts.Services.StateMachine.States
{
    public class RestartFsmState : FsmState
    {
        private readonly CellsManager _cellsManager;

        private readonly Defence _playerDefence;
        private readonly Health _playerHealth;
        private readonly Damage _playerDamage;
        private readonly Mana _playerMana;

        private readonly Defence _enemyDefence;
        private readonly Health _enemyHealth;
        private readonly Damage _enemyDamage;

        private readonly AvailableSpells _availableSpells;
        private readonly PlayerMovement _playerMovement;
        private readonly EnemyTargetController _enemyTargetController;

        public RestartFsmState(FiniteStateMachine finiteStateMachine, CellsManager cellsManager, Defence playerDefence,
            Health playerHealth, Damage playerDamage, Mana playerMana, Defence enemyDefence, Health enemyHealth,
            Damage enemyDamage, AvailableSpells availableSpells, PlayerMovement playerMovement,
            EnemyTargetController enemyTargetController)
            : base(finiteStateMachine)
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
        }

        public override void Enter()
        {
            base.Enter();

            _cellsManager.CleanAll();
            _cellsManager.FillWithEffects();

            _playerDefence.SetToDefault();
            _playerHealth.SetToDefault();
            _playerDamage.SetToDefault();
            _playerMana.SetToDefault();

            _availableSpells.Clear();

            _playerMovement.SetDefaultStayCell();

            _enemyDefence.SetToDefault();
            _enemyHealth.SetToDefault();
            _enemyDamage.SetToDefault();
            // TODO: задать дефолтную модель противника  

            _enemyTargetController.SetAimToNewRandomTargetCell();

            FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}