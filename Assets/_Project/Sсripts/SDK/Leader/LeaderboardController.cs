using System;
using System.Collections.Generic;
using _Project.View;
using Agava.YandexGames;
using UnityEngine;

namespace _Project.SDK.Leader
{
    public class LeaderboardController : IDisposable
    {
        private readonly LeaderboardButtonView _leaderboardButtonView;
        private readonly LeaderboardPopupView _leaderboardPopup;
        private readonly YandexLeaderBoard _yandexLeaderBoard;

        public LeaderboardController(LeaderboardButtonView leaderboardButtonView, LeaderboardPopupView leaderboardPopup,
            YandexLeaderBoard yandexLeaderBoard)
        {
            _leaderboardButtonView = leaderboardButtonView;
            _leaderboardPopup = leaderboardPopup;
            _yandexLeaderBoard = yandexLeaderBoard;

            _leaderboardButtonView.ButtonOnClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Open();
#else
            Test();
#endif
        }

        private void Test()
        {
            List<LeaderBoardPlayer> leaderBoardPlayers = new List<LeaderBoardPlayer>()
            {
                new LeaderBoardPlayer(12, "name1"),
                new LeaderBoardPlayer(10, "name2"),
                new LeaderBoardPlayer(8, "name3"),
                new LeaderBoardPlayer(6, "name4"),
            };

            _leaderboardPopup.ConstructLeaderboard(leaderBoardPlayers);
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

            List<LeaderBoardPlayer> leaderBoardPlayers = _yandexLeaderBoard.GetLeaderBoardPlayers();

            _leaderboardPopup.ConstructLeaderboard(leaderBoardPlayers);
        }

        public void Dispose()
        {
            _leaderboardButtonView.ButtonOnClick.AddListener(OnButtonClick);
        }
    }
}