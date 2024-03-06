using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Config;
using _Project.View;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.SDK.Leader
{
    public class LeaderboardController : IDisposable
    {
        private readonly LeaderboardButtonView _leaderboardButtonView;
        private readonly LeaderboardPopupView _leaderboardPopup;
        private readonly YandexLeaderBoard _yandexLeaderBoard;
        private readonly Coefficients _coefficients;

        public LeaderboardController(LeaderboardButtonView leaderboardButtonView, LeaderboardPopupView leaderboardPopup,
            YandexLeaderBoard yandexLeaderBoard, Coefficients coefficients)
        {
            Assert.IsNotNull(leaderboardButtonView);
            Assert.IsNotNull(leaderboardPopup);
            Assert.IsNotNull(yandexLeaderBoard);
            Assert.IsNotNull(coefficients);

            _leaderboardButtonView = leaderboardButtonView;
            _leaderboardPopup = leaderboardPopup;
            _yandexLeaderBoard = yandexLeaderBoard;
            _coefficients = coefficients;

            _leaderboardButtonView.ButtonOnClick.AddListener(OnButtonClick);
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

        private void Open()
        {
            Debug.Log("PlayerAccount.Authorize() - STARTED");
            PlayerAccount.Authorize();

            if (PlayerAccount.IsAuthorized)
            {
                Debug.Log("PlayerAccount.RequestPersonalProfileDataPermission() - STARTED");
                PlayerAccount.RequestPersonalProfileDataPermission();
            }

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("PlayerAccount.IsAuthorized == false");
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

        public void Dispose()
        {
            _leaderboardButtonView.ButtonOnClick.AddListener(OnButtonClick);
        }
    }
}