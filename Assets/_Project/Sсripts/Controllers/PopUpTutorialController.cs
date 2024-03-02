using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Sсripts.View;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
{
    public class PopUpTutorialController
    {
        private readonly PopUpNotificationView _popUp;
        private readonly Dictionary<TutorialStep, PopUpNotificationModel> _tutorialContent;

        private bool _isClosed = true;

        public PopUpTutorialController(PopUpNotificationView popUp,
            Dictionary<TutorialStep, PopUpNotificationModel> tutorialContent)
        {
            Assert.IsNotNull(popUp);
            Assert.IsNotNull(tutorialContent);

            _popUp = popUp;
            _tutorialContent = tutorialContent;
            IsEnable = true;
        }

        public bool IsEnable { get; private set; }

        public void Switch(bool isEnabled)
        {
            IsEnable = isEnabled;
        }

        public async Task Show(TutorialStep tutorialStep)
        {
            if (!IsEnable) return;

            _isClosed = false;

            PopUpNotificationModel model = _tutorialContent[tutorialStep];

            _popUp.SetContent(model.Title, model.Info);
            _popUp.Button.onClick.AddListener(Hide);
            _popUp.Show();

            if (tutorialStep == TutorialStep.LastTip)
            {
                IsEnable = false;
            }

            await UniTask.WaitUntil(() => _isClosed);
        }

        private void Hide()
        {
            _popUp.Button.onClick.RemoveListener(Hide);
            _popUp.Hide();
            _isClosed = true;
        }
    }
}