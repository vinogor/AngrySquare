using System;
using System.Threading.Tasks;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.SDK
{
    public class Advertising
    {
        private bool _isAdClosed = false;
        public event Action<bool> SwitchSound;

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

            SwitchSound?.Invoke(true);
            _isAdClosed = true;
        }

        private void OnOpenCallBack()
        {
            SwitchSound?.Invoke(false);
        }
    }
}