using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Config;
using _Project.Domain.Effects;
using _Project.Domain.Movement;
using _Project.View;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Controllers.PopupChoice
{
    public class PopUpChoiceEffectController : PopUpChoiceAbstractController
    {
        private readonly List<EffectName> _availableEffectNames;
        private readonly PlayerMovement _playerMovement;
        private readonly CellsSettings _cellsSettings;

        public PopUpChoiceEffectController(PopUpChoiceView popUpChoiceView, List<EffectName> availableEffectNames,
            PlayerMovement playerMovement, CellsSettings cellsSettings) : base(popUpChoiceView)
        {
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(cellsSettings);

            _availableEffectNames = availableEffectNames;
            _playerMovement = playerMovement;
            _cellsSettings = cellsSettings;
        }

        public event Action Showed;
        public event Action Clicked;

        public void ShowEffects()
        {
            PrepareEffectButtons();
            Showed?.Invoke();
            PopUpChoiceView.Show();
        }

        private void PrepareEffectButtons()
        {
            EffectName[] effectNames = SelectRandomItems(_availableEffectNames);
            Sprite[] sprites = effectNames.Select(name => _cellsSettings.GetCellSprite(name)).ToArray();

            PopUpChoiceView.SetSprites(sprites);

            for (var i = 0; i < PopUpChoiceView.ButtonsOnClick.Length; i++)
            {
                EffectName effectName = effectNames[i];
                PopUpChoiceView.ButtonsOnClick[i].AddListener(() => PlayEffect(effectName));
            }
        }

        private void PlayEffect(EffectName effectName)
        {
            HidePopup();
            Clicked?.Invoke();
            _playerMovement.ActivateEffect(effectName);
        }
    }
}