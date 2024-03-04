using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

namespace _Project.SDK.Leader
{
    public class YandexLeaderBoard
    {
        private const string AnonymousName = "Anonymous";
        private const string LeaderBoardName = "LeaderBoardAngrySquare";

        private readonly List<LeaderBoardPlayer> _leaderBoardPlayers = new();

        public void SetPlayer(int score)
        {
            Debug.Log("YandexLeaderBoard - SetPlayer - " + score + ", start...");

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("YandexLeaderBoard - SetPlayer - IsAuthorized == false");
                return;
            }

            Leaderboard.GetPlayerEntry(LeaderBoardName,
                onSuccessCallback =>
                {
                    Leaderboard.SetScore(LeaderBoardName, score);
                    Debug.Log("YandexLeaderBoard - SetPlayer - Success");
                });

            Fill();
        }

        public List<LeaderBoardPlayer> GetLeaderBoardPlayers()
        {
            Fill();
            return _leaderBoardPlayers;
        }

        private void Fill()
        {
            Debug.Log("YandexLeaderBoard - Fill - start...");
            _leaderBoardPlayers.Clear();

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("YandexLeaderBoard - Fill - IsAuthorized == false");
                return;
            }

            Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - start...");
            Leaderboard.GetEntries(LeaderBoardName, onSuccessCallback: result =>
                {
                    Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - Success");

                    foreach (var entry in result.entries)
                    {
                        var score = entry.score;
                        var name = entry.player.publicName;

                        if (string.IsNullOrEmpty(name))
                            name = AnonymousName;

                        _leaderBoardPlayers.Add(new LeaderBoardPlayer(score, name));
                    }
                },
                (onErrorCallback) =>
                {
                    Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - Error: " + onErrorCallback);
                });
        }
    }
}