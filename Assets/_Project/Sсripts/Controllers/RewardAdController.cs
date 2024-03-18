using System;
using Cysharp.Threading.Tasks;
using Domain;
using SDK;
using UnityEngine;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class RewardAdController : IDisposable
    {

        private readonly RewardAdView _rewardAdView;
        private readonly Advertising _advertising;
        private readonly Mana _mana;
        private readonly FrameScaler _frameScaler;

        private bool _isTimerRun;
        private bool _isShowed;

        public RewardAdController(RewardAdView rewardAdView, Advertising advertising, Mana mana, FrameScaler frameScaler)
        {
            Assert.IsNotNull(rewardAdView);
            Assert.IsNotNull(advertising);
            Assert.IsNotNull(mana);
            Assert.IsNotNull(frameScaler);

            _rewardAdView = rewardAdView;
            _advertising = advertising;
            _mana = mana;
            _frameScaler = frameScaler;

            _mana.ValueChanged += RunTimer;

            _isTimerRun = false;

            if (_mana.Value < _mana.MaxValue)
            {
                Debug.Log("RewardAdController - ON START _mana.Value <= _mana.MaxValue, so SHOW");
                Show();
            }
            else
            {
                Debug.Log("RewardAdController - ON START _mana.Value == _mana.MaxValue, so HIDE");
                Hide();
            }
        }

        private void Show()
        {
            Debug.Log("RewardAdController - Show");
            _rewardAdView.Show();
            _isShowed = true;
            _rewardAdView.Clicked += OnClick;
            _frameScaler.Enable();
        }

        private void Hide()
        {
            Debug.Log("RewardAdController - Hide");
            _frameScaler.Disable();
            _rewardAdView.Hide();
            _rewardAdView.Clicked -= OnClick;

            _isShowed = false;
            _isTimerRun = false;
            
            Debug.Log("RewardAdController - _isTimerRun set false");
        }

        private async void OnClick()
        {
            Debug.Log("RewardAdController - OnClick");

            Hide();

            UniTask<bool> showRewardedAd = _advertising.ShowRewardedAd();
            await showRewardedAd;
            bool wasRewarded = showRewardedAd.GetAwaiter().GetResult();

            Debug.Log("RewardAdController - OnClick - wasRewarded = " + wasRewarded);

            if (wasRewarded)
            {
                _mana.ReplenishToMax();
                _isTimerRun = false;
                Debug.Log("RewardAdController - set _isTimerRun = false after replenish");
            }
        }

        private async void RunTimer(int _)
        {
            Debug.Log("RewardAdController - mana value changed!");

            if (_isShowed && _mana.Value == _mana.MaxValue)
            {
                Debug.Log("RewardAdController - _isShowed = true and _mana.MaxValue, so HIDE");
                Hide();
            }

            if (_isShowed)
            {
                Debug.Log("RewardAdController - _isShowed = true, so return");
                return;
            }

            if (_isTimerRun && _mana.Value == _mana.MaxValue)
            {
                Debug.Log(
                    "RewardAdController - _isTimerRun = true, and _mana.MaxValue so return adn set _isTimerRun to false");
                _isTimerRun = false;
                Hide();
                return;
            }

            if (_isTimerRun)
            {
                Debug.Log("RewardAdController - _isTimerRun = true, so return");
                return;
            }

            if (_mana.Value == _mana.MaxValue)
            {
                Debug.Log("RewardAdController - _mana.Value == _mana.MaxValue, so return");
                return;
            }

            _isTimerRun = true;
            Debug.Log("RewardAdController - _isTimerRun set true and run waiting");

            await UniTask.WaitForSeconds(60);

            if (_mana.Value <= _mana.MaxValue && _isTimerRun == true)
            {
                Debug.Log("RewardAdController - AFTER waiting _mana.Value <= _mana.MaxValue, so SHOW");
                Show();
            }
        }

        public void Dispose()
        {
            _mana.ValueChanged -= RunTimer;
        }
    }
}