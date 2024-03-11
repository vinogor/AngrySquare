using System;
using System.Collections.Generic;
using System.Linq;
using Services.Utility;
using UnityEngine.Assertions;
using View;

namespace Controllers.PopupChoice
{
    public abstract class PopUpChoiceAbstractPresenter
    {
        protected readonly PopUpChoiceView PopUpChoiceView;

        private const int TotalAmountItems = 3;

        protected PopUpChoiceAbstractPresenter(PopUpChoiceView popUpChoiceView)
        {
            Assert.IsNotNull(popUpChoiceView);
            PopUpChoiceView = popUpChoiceView;
            PopUpChoiceView.Hide();
        }

        public void HidePopup()
        {
            Array.ForEach(PopUpChoiceView.ButtonsOnClick, onClick => onClick.RemoveAllListeners());
            PopUpChoiceView.Hide();
        }
        
        protected T[] SelectRandomItems<T>(IEnumerable<T> collection)
        {
            return collection.Shuffle().Take(TotalAmountItems).ToArray();
        }
    }
}