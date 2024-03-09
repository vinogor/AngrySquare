using System.Collections.Generic;
using System.Linq;
using Config;
using Controllers.Sound;
using Domain.Effects;
using Domain.Movement;
using UnityEngine;
using UnityEngine.Assertions;
using View;

namespace Controllers.PopupChoice
{
    public class PopUpChoiceEffectPresenter : PopUpChoiceAbstractPresenter
    {
        private readonly List<EffectName> _availableEffectNames;
        private readonly PlayerMovement _playerMovement;
        private readonly CellsSettings _cellsSettings;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        public PopUpChoiceEffectPresenter(PopUpChoiceView popUpChoiceView, List<EffectName> availableEffectNames,
            PlayerMovement playerMovement, CellsSettings cellsSettings, GameSoundsPresenter gameSoundsPresenter) : base(popUpChoiceView)
        {
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(cellsSettings);
            Assert.IsNotNull(gameSoundsPresenter);

            _availableEffectNames = availableEffectNames;
            _playerMovement = playerMovement;
            _cellsSettings = cellsSettings;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public void ShowEffects()
        {
            PrepareEffectButtons();
            _gameSoundsPresenter.PlayPopUpShowed();
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
            _gameSoundsPresenter.PlayClickButton();
            _playerMovement.ActivateEffect(effectName);
        }
    }
}