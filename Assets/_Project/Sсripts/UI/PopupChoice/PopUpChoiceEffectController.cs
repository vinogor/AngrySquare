using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI.PopupChoice
{
    public class PopUpChoiceEffectController : PopUpChoiceAbstractController
    {
        private readonly List<EffectName> _availableEffectNames;
        private readonly PlayerMovement _playerMovement;
        private readonly CellsSettings _cellsSettings;

        public PopUpChoiceEffectController(PopUpChoice popUpChoice, List<EffectName> availableEffectNames,
            PlayerMovement playerMovement, CellsSettings cellsSettings) : base(popUpChoice)
        {
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(cellsSettings);

            _availableEffectNames = availableEffectNames;
            _playerMovement = playerMovement;
            _cellsSettings = cellsSettings;
        }

        public void ShowEffects()
        {
            PrepareEffectButtons();
            PopUpChoice.Show();
        }

        private void PrepareEffectButtons()
        {
            EffectName[] effectNames = SelectRandomItems(_availableEffectNames);
            Sprite[] sprites = effectNames.Select(name => _cellsSettings.GetCellSprite(name)).ToArray();

            PopUpChoice.SetSprites(sprites);

            for (var i = 0; i < PopUpChoice.ButtonsOnClick.Length; i++)
            {
                EffectName effectName = effectNames[i];
                PopUpChoice.ButtonsOnClick[i].AddListener(() => PlayEffect(effectName));
            }
        }

        private void PlayEffect(EffectName effectName)
        {
            HidePopup();
            _playerMovement.ActivateEffect(effectName);
        }
    }
}