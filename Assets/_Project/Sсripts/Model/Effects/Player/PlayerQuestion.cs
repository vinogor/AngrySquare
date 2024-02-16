using System;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects.Player
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
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.PlayerJumpInPlace());
            sequence.AppendCallback(_popUpQuestion.Show);
            // sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
            
            // считать какой эффект был выбран (пока они всегда одинаковые)
            // применить выбранный эффект
            // и после этого завершающий коллбэк
            
        }
    }
}