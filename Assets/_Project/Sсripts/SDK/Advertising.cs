using System.Threading.Tasks;
using _Project.Sсripts.Controllers.Sound;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Sсripts.SDK
{
    public class Advertising
    {
        private readonly GameSounds _gameSounds;

        private bool _isAdClosed;

        public Advertising(GameSounds gameSounds)
        {
            _gameSounds = gameSounds;
            _isAdClosed = false;
        }

        public async Task ShowAd()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            await Show();
#endif
        }

        private async Task Show()
        {
            Debug.Log("Advertising - Show - start");
            VideoAd.Show(OnOpenCallBack, OnRewardedCallback, OnCloseCallback);
            Debug.Log("Advertising - Show - end");
            await UniTask.WaitUntil(() => _isAdClosed);
        }

        private void OnOpenCallBack()
        {
            _gameSounds.Switch(false);
        }

        private void OnCloseCallback()
        {
            _gameSounds.Switch(true);
            _isAdClosed = true;
        }

        private void OnRewardedCallback()
        {
        }
    }
}