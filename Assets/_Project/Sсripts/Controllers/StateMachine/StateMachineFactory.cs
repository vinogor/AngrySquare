using Controllers.Sound;
using Controllers.StateMachine.States;
using Domain;
using Domain.Movement;
using SDK;
using SDK.Leader;
using Services;
using UnityEngine.Assertions;

namespace Controllers.StateMachine
{
    public static class StateMachineFactory
    {
        public static FiniteStateMachine CreateStateMachine(SpellBarController spellBarController,
            TutorialController tutorialController, PopUpNotificationController popUpPlayerWin,
            PopUpNotificationController popUpPlayerDefeat, Advertising advertising, DiceRoller diceRoller,
            PlayerMovement playerMovement, Health playerHealth, Health enemyHealth, LevelRestarter levelRestarter,
            GameSoundsPresenter gameSoundsPresenter, EnemyMovement enemyMovement, YandexLeaderBoard yandexLeaderBoard)
        {
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(tutorialController);
            Assert.IsNotNull(popUpPlayerWin);
            Assert.IsNotNull(popUpPlayerDefeat);
            Assert.IsNotNull(popUpPlayerDefeat);
            Assert.IsNotNull(advertising);
            Assert.IsNotNull(diceRoller);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(levelRestarter);
            Assert.IsNotNull(gameSoundsPresenter);
            Assert.IsNotNull(enemyMovement);
            Assert.IsNotNull(yandexLeaderBoard);
            
            FiniteStateMachine stateMachine = new FiniteStateMachine();
            GameInitializeFsmState gameInitializeFsmState = new GameInitializeFsmState(stateMachine, advertising);
            PlayerTurnSpellFsmState playerTurnSpellFsmState =
                new PlayerTurnSpellFsmState(stateMachine, spellBarController, tutorialController);
            PlayerTurnMoveFsmState playerTurnMoveFsmState = new PlayerTurnMoveFsmState(stateMachine, diceRoller,
                playerMovement, enemyHealth, tutorialController);
            PlayerWinFsmState playerWinFsmState =
                new PlayerWinFsmState(stateMachine, popUpPlayerWin, levelRestarter, gameSoundsPresenter);
            EnemyTurnFsmState enemyTurnFsmState = new EnemyTurnFsmState(stateMachine, enemyMovement, playerHealth,
                tutorialController);
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