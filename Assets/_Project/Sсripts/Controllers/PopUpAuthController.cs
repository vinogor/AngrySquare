using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class PopUpAuthController
    {
        private readonly PopUpAuthView _popUpAuthView;

        private bool _isClosed;
        private bool _isReadyToAuthorize;

        public PopUpAuthController(PopUpAuthView popUpAuthView)
        {
            Assert.IsNotNull(popUpAuthView);

            _popUpAuthView = popUpAuthView;
            _popUpAuthView.Clicked += OnClicked;
            Hide();
        }

        public async UniTask<bool> Show()
        {
            _popUpAuthView.Show();
            _isClosed = false;

            await UniTask.WaitUntil(() => _isClosed);

            return _isReadyToAuthorize;
        }

        private void OnClicked(bool isReadyToAuthorize)
        {
            Hide();
            _isReadyToAuthorize = isReadyToAuthorize;
            _isClosed = true;
        }

        private void Hide()
        {
            _popUpAuthView.Hide();
        }
    }
}