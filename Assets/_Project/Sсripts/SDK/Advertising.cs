using System.Threading.Tasks;
using _Project.Controllers.Sound;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.SDK
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
            InterstitialAd.Show(OnOpenCallBack, OnCloseCallback);
            Debug.Log("Advertising - Show - end");
            await UniTask.WaitUntil(() => _isAdClosed);
        }

        private void OnCloseCallback(bool flag)
        {
            Debug.Log("Advertising - OnCloseCallback - " + flag);
            _gameSounds.SwitchByAdv(true);
            _isAdClosed = true;
        }

        private void OnOpenCallBack()
        {
            _gameSounds.SwitchByAdv(false);
        }
    }
}