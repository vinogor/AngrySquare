using System.Linq;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class PopUpQuestionController
    {
        private readonly PopUpQuestion _popUpQuestion;
        private readonly CellsAndSpellsSettings _cellsAndSpellsSettings;
        private readonly EffectName[] _availableEffectNames;
        private readonly PlayerMovement _playerMovement;

        private EffectName[] _activeEffects;

        public PopUpQuestionController(PopUpQuestion popUpQuestion, CellsAndSpellsSettings cellsAndSpellsSettings,
            EffectName[] availableEffectNames, PlayerMovement playerMovement)
        {
            Assert.IsNotNull(popUpQuestion);
            Assert.IsNotNull(cellsAndSpellsSettings);
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);

            _popUpQuestion = popUpQuestion;
            _cellsAndSpellsSettings = cellsAndSpellsSettings;
            _availableEffectNames = availableEffectNames;
            _playerMovement = playerMovement;
        }

        public void Show()
        {
            PrepareButtons();
            _popUpQuestion.Show();
        }

        private void PrepareButtons()
        {
            EffectName[] effects = SelectRandomEffects();
            Button[] buttons = _popUpQuestion.Buttons;

            for (var i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                EffectName effectName = effects[i];
                button.GetComponentInChildren<QuestionImage>().GetComponent<Image>().sprite =
                    _cellsAndSpellsSettings.GetCellSprite(effectName);

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => PlayEffect(effectName));
            }
        }

        private void PlayEffect(EffectName effectName)
        {
            _popUpQuestion.Hide();
            Debug.Log("BUTTON = Playing effect: " + effectName);
            _playerMovement.ActivateEffect(effectName);
        }

        private EffectName[] SelectRandomEffects()
        {
            return _availableEffectNames.Shuffle().Take(3).ToArray();
        }
    }
}