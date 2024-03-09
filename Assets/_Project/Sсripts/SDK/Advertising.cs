using Agava.YandexGames;
using Controllers.Sound;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace SDK
{
    public class Advertising
    {
        private bool _isAdClosed = false;

        private readonly GameSoundsPresenter _gameSoundsPresenter;

        public Advertising(GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(gameSoundsPresenter);
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public async UniTask ShowAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            await Show();
#endif
        }

        private async UniTask Show()
        {
            Debug.Log("Advertising - Show - start");
            InterstitialAd.Show(OnOpenCallBack, OnCloseCallback);
            Debug.Log("Advertising - Show - end");
            await UniTask.WaitUntil(() => _isAdClosed);
        }

        private void OnCloseCallback(bool flag)
        {
            Debug.Log("Advertising - OnCloseCallback - " + flag);

            _gameSoundsPresenter.SwitchByAdv(true);
            _isAdClosed = true;
        }

        private void OnOpenCallBack()
        {
            _gameSoundsPresenter.SwitchByAdv(false);
        }
    }
}