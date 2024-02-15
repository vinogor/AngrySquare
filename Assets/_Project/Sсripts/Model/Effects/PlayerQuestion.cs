using System;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerQuestion : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpQuestion _popUpQuestion;

        public PlayerQuestion(PlayerJumper playerJumper, PopUpQuestion popUpQuestion)
        {
            _playerJumper = playerJumper;
            _popUpQuestion = popUpQuestion;
        }

        protected override void Execute(Action onComplete)
        {
            _playerJumper.PlayerJumpInPlace(() =>
            {
                _popUpQuestion.SetActive();
            });
            
            
            // считать какой эффект был выбран (пока они всегда одинаковые)
            // применить выбранный эффект
            // и после этого завершающий коллбэк
            
            onComplete.Invoke();
        }
    }
}