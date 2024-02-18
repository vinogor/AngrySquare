using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Utility;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI.PopupChoice
{
    public abstract class PopUpChoiceAbstractController
    {
        protected readonly PopUpChoice PopUpChoice;

        private const int AmountItems = 3;

        protected PopUpChoiceAbstractController(PopUpChoice popUpChoice)
        {
            Assert.IsNotNull(popUpChoice);

            PopUpChoice = popUpChoice;
        }

        protected void HidePopup()
        {
            Array.ForEach(PopUpChoice.ButtonsOnClick, onClick => onClick.RemoveAllListeners());
            PopUpChoice.Hide();
        }
        
        protected List<T> SelectRandomItems<T>(List<T> collection)
        {
            return collection.Shuffle().Take(AmountItems).ToList();
        }
    }
}