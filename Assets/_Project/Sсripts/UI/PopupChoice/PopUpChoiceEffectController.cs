using System.Linq;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.UI.PopupChoice
{
    public class PopUpChoiceEffectController : PopUpChoiceAbstractController
    {
        private readonly EffectName[] _availableEffectNames;
        private readonly PlayerMovement _playerMovement;
        private readonly CellsSettings _cellsSettings;

        public PopUpChoiceEffectController(PopUpChoice popUpChoice, EffectName[] availableEffectNames,
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
            EffectName[] effectNames = SelectRandomEffects();
            Button[] buttons = PopUpChoice.Buttons;
            Sprite[] sprites = effectNames.Select(name => _cellsSettings.GetCellSprite(name)).ToArray();

            PopUpChoice.SetSprites(sprites);

            for (var i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                EffectName effectName = effectNames[i];
                button.onClick.AddListener(() => PlayEffect(effectName));
            }
        }

        private void PlayEffect(EffectName effectName)
        {
            HidePopup();
            _playerMovement.ActivateEffect(effectName);
        }

        private EffectName[] SelectRandomEffects()
        {
            return _availableEffectNames.Shuffle().Take(AmountItems).ToArray();
        }
    }
}