using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Domain;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.SDK.Leader
{
    public class YandexLeaderBoard : IDisposable
    {
        private readonly EnemyLevel _enemyLevel;
        private const string AnonymousName = "Anonymous";
        private const string LeaderBoardName = "LeaderBoardAngrySquare";

        public List<LeaderBoardPlayer> LeaderBoardPlayers { get; private set; }

        public YandexLeaderBoard(EnemyLevel enemyLevel)
        {
            LeaderBoardPlayers = new List<LeaderBoardPlayer>();
            _enemyLevel = enemyLevel;

            _enemyLevel.Changed += EnemyLevelOnChanged();
            _enemyLevel.SetDefault += EnemyLevelOnChanged();
        }

        public void Dispose()
        {
            _enemyLevel.Changed -= EnemyLevelOnChanged();
            _enemyLevel.SetDefault -= EnemyLevelOnChanged();
        }

        private Action EnemyLevelOnChanged()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return () => SetScore(_enemyLevel.Value);
#else
            return () => { };
#endif
        }

        public async void SetScore(int score)
        {
            Debug.Log("YandexLeaderBoard - SetPlayer - " + score + ", start...");

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("YandexLeaderBoard - SetPlayer - IsAuthorized == false");
                return;
            }

            bool isSetScoreCompleted = false;
            Leaderboard.GetPlayerEntry(LeaderBoardName,
                onSuccessCallback =>
                {
                    Leaderboard.SetScore(LeaderBoardName, score);
                    Debug.Log("YandexLeaderBoard - SetPlayer - Success, score = " + score);
                    isSetScoreCompleted = true;
                }, onErrorCallback =>
                {
                    Debug.Log("YandexLeaderBoard - SetPlayer - Error: " + onErrorCallback);
                    isSetScoreCompleted = true;
                });

            await UniTask.WaitUntil(() => isSetScoreCompleted);
            await Fill();
        }

        private async Task Fill()
        {
            Debug.Log("YandexLeaderBoard - Fill - start...");
            LeaderBoardPlayers.Clear();

            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("YandexLeaderBoard - Fill - IsAuthorized == false");
                return;
            }

            Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - start...");

            bool loadComplete = false;
            Leaderboard.GetEntries(LeaderBoardName, onSuccessCallback: result =>
                {
                    Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - Success");

                    foreach (var entry in result.entries)
                    {
                        var score = entry.score;
                        var name = entry.player.publicName;
                        Debug.Log($"YandexLeaderBoard - Leaderboard.GetEntries - name = {name}, score = {score}");

                        if (string.IsNullOrEmpty(name))
                            name = AnonymousName;

                        LeaderBoardPlayers.Add(new LeaderBoardPlayer(score, name));
                    }

                    Debug.Log(
                        "YandexLeaderBoard - Leaderboard.GetEntries - loadComplete = true, _leaderBoardPlayers.Count = " +
                        LeaderBoardPlayers.Count);
                    loadComplete = true;
                },
                (onErrorCallback) =>
                {
                    Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - Error: " + onErrorCallback);
                    Debug.Log("YandexLeaderBoard - Leaderboard.GetEntries - loadComplete = true");
                    loadComplete = true;
                });

            await UniTask.WaitUntil(() => loadComplete);
        }
    }
}