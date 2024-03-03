using System.Collections.Generic;
using _Project.Controllers;

namespace _Project.Config
{
    public static class UiTextKeys
    {
        private const string TutorialIntroTitleKey = "Tutorial - Intro - Title";
        private const string TutorialIntroInfoKey = "Tutorial - Intro - Info";
        private const string TutorialSpellCastTitleKey = "Tutorial - SpellCast - Title";
        private const string TutorialSpellCastInfoKey = "Tutorial - SpellCast - Info";
        private const string TutorialRollDiceTitleKey = "Tutorial - RollDice - Title";
        private const string TutorialRollDiceInfoKey = "Tutorial - RollDice - Info";
        private const string TutorialEnemyTurnTitleKey = "Tutorial - EnemyTurn - Title";
        private const string TutorialEnemyTurnInfoKey = "Tutorial - EnemyTurn - Info";
        private const string TutorialLastTipTitleKey = "Tutorial - LastTip - Title";
        private const string TutorialLastTipInfoKey = "Tutorial - LastTip - Info";
        
        public const string NotificationPlayerWinTitleKey = "Notification - PlayerWin - Title";
        public const string NotificationPlayerWinInfoKey = "Notification - PlayerWin - Info"; 
        public const string NotificationPlayerDefeatTitleKey = "Notification - PlayerDefeat - Title";
        public const string NotificationPlayerDefeatInfoKey = "Notification - PlayerDefeat - Info";
        
        public const string ButtonSkipSpellKey = "Button - SkipSpell";
        public const string EnemyLevelKey = "Stats - Enemy level";
        public const string SpellBarCostKey = "SpellBar - Cost";
        
        private static Dictionary<TutorialStep, PopUpNotificationModel> s_tutorial = new()
        {
            { TutorialStep.Intro, new PopUpNotificationModel(TutorialIntroTitleKey, TutorialIntroInfoKey) },
            { TutorialStep.SpellCast, new PopUpNotificationModel(TutorialSpellCastTitleKey, TutorialSpellCastInfoKey) },
            { TutorialStep.RollDice, new PopUpNotificationModel(TutorialRollDiceTitleKey, TutorialRollDiceInfoKey) },
            { TutorialStep.EnemyTurn, new PopUpNotificationModel(TutorialEnemyTurnTitleKey, TutorialEnemyTurnInfoKey) },
            { TutorialStep.LastTip, new PopUpNotificationModel(TutorialLastTipTitleKey, TutorialLastTipInfoKey) },
        };

        public static PopUpNotificationModel Get(TutorialStep tutorialStep)
        {
            return s_tutorial[tutorialStep];
        }
    }
}