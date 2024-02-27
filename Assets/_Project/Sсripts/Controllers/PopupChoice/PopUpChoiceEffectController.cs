using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Domain.Movement;
using _Project.Sсripts.View;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers.PopupChoice
{
    public class PopUpChoiceEffectController : PopUpChoiceAbstractController
    {
        private readonly List<EffectName> _availableEffectNames;
        private readonly PlayerMovement _playerMovement;
        private readonly CellsSettings _cellsSettings;
        private readonly GameSounds _gameSounds;

        public PopUpChoiceEffectController(PopUpChoiceView popUpChoiceView, List<EffectName> availableEffectNames,
            PlayerMovement playerMovement, CellsSettings cellsSettings, GameSounds gameSounds) : base(popUpChoiceView)
        {
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(cellsSettings);
            Assert.IsNotNull(gameSounds);

            _availableEffectNames = availableEffectNames;
            _playerMovement = playerMovement;
            _cellsSettings = cellsSettings;
            _gameSounds = gameSounds;
        }

        public void ShowEffects()
        {
            PrepareEffectButtons();
            _gameSounds.PlayPopUp();
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
            _gameSounds.PlayClickButton();
            _playerMovement.ActivateEffect(effectName);
        }
    }
}