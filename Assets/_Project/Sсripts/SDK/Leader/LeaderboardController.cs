using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using Config;
using Controllers;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.Assertions;
using View;

namespace SDK.Leader
{
    public class LeaderboardController : IPresenter
    {
        private readonly LeaderboardButtonView _leaderboardButtonView;
        private readonly LeaderboardPopupView _leaderboardPopup;
        private readonly YandexLeaderBoard _yandexLeaderBoard;
        private readonly PopUpAuthController _popUpAuthController;
        private readonly Coefficients _coefficients;

        public LeaderboardController(LeaderboardButtonView leaderboardButtonView, LeaderboardPopupView leaderboardPopup,
            YandexLeaderBoard yandexLeaderBoard, PopUpAuthController popUpAuthController, Coefficients coefficients)
        {
            Assert.IsNotNull(leaderboardButtonView);
            Assert.IsNotNull(leaderboardPopup);
            Assert.IsNotNull(yandexLeaderBoard);
            Assert.IsNotNull(popUpAuthController);
            Assert.IsNotNull(coefficients);

            _leaderboardButtonView = leaderboardButtonView;
            _leaderboardPopup = leaderboardPopup;
            _yandexLeaderBoard = yandexLeaderBoard;
            _popUpAuthController = popUpAuthController;
            _coefficients = coefficients;
        }

        public void Enable()
        {
            _leaderboardButtonView.Clicked += OnButtonClick;
        }

        public void Disable()
        {
            _leaderboardButtonView.Clicked -= OnButtonClick;
        }

        private void OnButtonClick()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Open();
#else
            MockForLocal();
#endif
        }

        private void MockForLocal()
        {
            List<LeaderBoardPlayer> leaderBoardPlayers = new List<LeaderBoardPlayer>()
            {
                new LeaderBoardPlayer(12, "name1"),
                new LeaderBoardPlayer(10, "name2"),
                new LeaderBoardPlayer(8, "name3"),
                new LeaderBoardPlayer(6, "name4"),
            };
            var limitedPlayers = LimitAmount(leaderBoardPlayers);
            _leaderboardPopup.ConstructLeaderboard(limitedPlayers);
        }

        private async void Open()
        {
            Debug.Log("PlayerAccount - Open - STARTED");

            if (PlayerAccount.IsAuthorized)
            {
                Debug.Log("PlayerAccount - 1 - IsAuthorized == true");

                Debug.Log("PlayerAccount.RequestPersonalProfileDataPermission()");
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("PlayerAccount - IsAuthorized == false, show popUp");

                UniTask<bool> task = _popUpAuthController.Show();
                await task;

                bool isReadyToAuthorize = task.GetAwaiter().GetResult();

                Debug.Log("PlayerAccount - popup - isReadyToAuthorize == " + isReadyToAuthorize);

                if (isReadyToAuthorize)
                {
                    Debug.Log("PlayerAccount - try to auth");
                    PlayerAccount.Authorize();
                }
                else
                {
                    Debug.Log("PlayerAccount - popup - return ");
                }
                
                return;
            }

            List<LeaderBoardPlayer> leaderBoardPlayers = _yandexLeaderBoard.LeaderBoardPlayers;

            Debug.Log("leaderBoardPlayers from yandex: " + leaderBoardPlayers.Count);

            List<LeaderBoardPlayer> limitedPlayers = LimitAmount(leaderBoardPlayers);
            Debug.Log("limitedPlayers from yandex: " + limitedPlayers.Count);

            _leaderboardPopup.ConstructLeaderboard(limitedPlayers);
        }

        private List<LeaderBoardPlayer> LimitAmount(List<LeaderBoardPlayer> leaderBoardPlayers)
        {
            return leaderBoardPlayers
                .Take(Mathf.Min(leaderBoardPlayers.Count, _coefficients.LeaderboardMaxPlayersToShow)).ToList();
        }
    }
}