using System;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI.PopupChoice
{
    public abstract class PopUpChoiceAbstractController
    {
        // TODO: вынести сюда общее из наследников
        
        protected readonly PopUpChoice PopUpChoice;

        protected const int AmountItems = 3;

        protected PopUpChoiceAbstractController(PopUpChoice popUpChoice)
        {
            Assert.IsNotNull(popUpChoice);

            PopUpChoice = popUpChoice;
        }
        
        protected void HidePopup()
        {
            Array.ForEach(PopUpChoice.Buttons, button => button.onClick.RemoveAllListeners());
            PopUpChoice.Hide();
        }
    }
}