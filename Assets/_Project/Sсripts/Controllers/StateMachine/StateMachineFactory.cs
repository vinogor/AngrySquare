using Controllers.Sound;
using Controllers.StateMachine.States;
using Domain;
using Domain.Movement;
using SDK;
using SDK.Leader;
using Services;

namespace Controllers.StateMachine
{
    public static class StateMachineFactory
    {
        public static FiniteStateMachine CreateStateMachine(SpellBarController spellBarController,
            PopUpTutorialController popUpTutorialController, PopUpNotificationController popUpNotificationController,
            PopUpNotificationController popUpPlayerDefeat, Advertising advertising, DiceRoller diceRoller,
            PlayerMovement playerMovement, Health playerHealth, Health enemyHealth, LevelRestarter levelRestarter,
            GameSoundsPresenter gameSoundsPresenter, EnemyMovement enemyMovement, YandexLeaderBoard yandexLeaderBoard)
        {
            FiniteStateMachine stateMachine = new FiniteStateMachine();
            GameInitializeFsmState gameInitializeFsmState = new GameInitializeFsmState(stateMachine, advertising);
            PlayerTurnSpellFsmState playerTurnSpellFsmState =
                new PlayerTurnSpellFsmState(stateMachine, spellBarController, popUpTutorialController);
            PlayerTurnMoveFsmState playerTurnMoveFsmState = new PlayerTurnMoveFsmState(stateMachine, diceRoller,
                playerMovement, enemyHealth, popUpTutorialController);
            PlayerWinFsmState playerWinFsmState =
                new PlayerWinFsmState(stateMachine, popUpNotificationController, levelRestarter, gameSoundsPresenter);
            EnemyTurnFsmState enemyTurnFsmState = new EnemyTurnFsmState(stateMachine, enemyMovement, playerHealth,
                popUpTutorialController);
            PlayerDefeatFsmState playerDefeatFsmState = new PlayerDefeatFsmState(stateMachine, popUpPlayerDefeat,
                levelRestarter, yandexLeaderBoard, gameSoundsPresenter);

            stateMachine.AddState(gameInitializeFsmState);
            stateMachine.AddState(playerTurnSpellFsmState);
            stateMachine.AddState(playerTurnMoveFsmState);
            stateMachine.AddState(playerWinFsmState);
            stateMachine.AddState(enemyTurnFsmState);
            stateMachine.AddState(playerDefeatFsmState);

            return stateMachine;
        }
    }
}