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

        public async UniTask ShowInterstitialAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            await ShowInterstitial();
#endif
        }

        public async UniTask<bool> ShowRewardedAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return await ShowRewarded();
#endif
            return true;
        }

        private async UniTask ShowInterstitial()
        {
            Debug.Log("Interstitial Advertising - Show - start");
            InterstitialAd.Show(OnOpenCallBack, OnCloseCallback);
            Debug.Log("Interstitial Advertising - Show - end");
            await UniTask.WaitUntil(() => _isAdClosed);
            _isAdClosed = false;
        }

        private async UniTask<bool> ShowRewarded()
        {
            Debug.Log("Rewarded Advertising - ShowRewarded - start");
            bool isRewarded = false;
            VideoAd.Show(OnOpenCallBack, () => isRewarded = true, OnCloseCallback);
            Debug.Log("Rewarded Advertising - ShowRewarded - end");
            await UniTask.WaitUntil(() => _isAdClosed);
            Debug.Log("Rewarded Advertising - ShowRewarded - after WaitUntil, isRewarded = " + isRewarded);
            _isAdClosed = false;
            return isRewarded;
        }

        private void OnCloseCallback(bool flag)
        {
            OnCloseCallback();
        }

        private void OnCloseCallback()
        {
            Debug.Log("Advertising - OnCloseCallback");

            _gameSoundsPresenter.SwitchByAdv(true);
            _isAdClosed = true;
        }

        private void OnOpenCallBack()
        {
            _gameSoundsPresenter.SwitchByAdv(false);
        }
    }
}