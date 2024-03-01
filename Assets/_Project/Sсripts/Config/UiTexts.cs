using _Project.Sсripts.Controllers;

namespace _Project.Sсripts.Config
{
    public static class UiTexts
    {
        public static readonly string TutorialIntroTitle = $"Tutorial - {TutorialStep.Intro}";
        public static readonly string TutorialIntroInfo = "It's like a board game. The player rolls the dice, his piece moves around the cells and activates various effects, including the ability to cast spells. The enemy sits in the center and jumps onto a cell chosen at random. The main goal is to defeat the enemy by damaging him until his life runs out.";
        
        public static readonly string TutorialSpellCastTitle = $"Tutorial - {TutorialStep.SpellCast}";
        public static readonly string TutorialSpellCastInfo = "The first step of a player's turn is to cast spells. You can only use one at a time, and only if you have enough mana for it. If you wish, you can skip this step altogether by clicking on the 'Skip' button. To cast, click on an available spell in the panel at the bottom of the screen.";
        
        
        public static readonly string TutorialRollDiceTitle = $"Tutorial - {TutorialStep.RollDice}";
        public static readonly string TutorialRollDiceInfo = "The next step is throwing the dice. Click on him to make him jump and throw a number. This will be the number of times your piece moves across the playing field. The cell on which the chip lands will be activated.";

        public static readonly string TutorialEnemyTurnTitle = $"Tutorial - {TutorialStep.EnemyTurn}";
        public static readonly string TutorialEnemyTurnInfo = "Now it will be the enemy's move. He will jump onto the cell that was marked with a red crosshair. Beware, if a player's piece is on it, it will receive increased damage! And if there is no player piece on it, then the opponent will also be able to activate some of the effects of the cells.";
        
        public static readonly string TutorialLastTipTitle = $"Tutorial - {TutorialStep.LastTip}";
        public static readonly string TutorialLastTipInfo = "That's all. Then the chain of actions will be repeated. The player casts a spell, then rolls the die and moves. The enemy follows and so on in a circle until someone runs out of life points. Good luck!";

    }
}