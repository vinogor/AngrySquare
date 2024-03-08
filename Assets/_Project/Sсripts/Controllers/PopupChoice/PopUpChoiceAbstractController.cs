using System;
using System.Collections.Generic;
using System.Linq;
using Services.Utility;
using UnityEngine.Assertions;
using View;

namespace Controllers.PopupChoice
{
    public abstract class PopUpChoiceAbstractController
    {
        protected readonly PopUpChoiceView PopUpChoiceView;

        private const int AmountItems = 3;

        protected PopUpChoiceAbstractController(PopUpChoiceView popUpChoiceView)
        {
            Assert.IsNotNull(popUpChoiceView);

            PopUpChoiceView = popUpChoiceView;
        }

        protected void HidePopup()
        {
            Array.ForEach(PopUpChoiceView.ButtonsOnClick, onClick => onClick.RemoveAllListeners());
            PopUpChoiceView.Hide();
        }
        
        protected T[] SelectRandomItems<T>(IEnumerable<T> collection)
        {
            return collection.Shuffle().Take(AmountItems).ToArray();
        }
    }
}