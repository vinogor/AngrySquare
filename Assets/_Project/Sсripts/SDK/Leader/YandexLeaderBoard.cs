using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agava.YandexGames;
using Controllers.StateMachine.States;
using Cysharp.Threading.Tasks;
using Domain;
using UnityEngine;

namespace SDK.Leader
{
    public class YandexLeaderBoard : IDisposable
    {
        private readonly EnemyLevel _enemyLevel;
        private readonly PlayerDefeatFsmState _playerDefeatFsmState;

        private const string AnonymousName = "Anonymous";
        private const string LeaderBoardName = "LeaderBoardAngrySquare";
        private string _currentPublicName;

        public YandexLeaderBoard(EnemyLevel enemyLevel, PlayerDefeatFsmState playerDefeatFsmState)
        {
            LeaderBoardPlayers = new List<LeaderBoardPlayer>();

            _enemyLevel = enemyLevel;
            _playerDefeatFsmState = playerDefeatFsmState;

            _playerDefeatFsmState.Defeat += OnDefeat;
        }

        public List<LeaderBoardPlayer> LeaderBoardPlayers { get; private set; }

        public void Dispose()
        {
            _playerDefeatFsmState.Defeat -= OnDefeat;
        }

        private void OnDefeat()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetScore(_enemyLevel.Value);
#endif
        }

        public async void SetScore(int score)
        {
            Debug.Log("YandexLeaderBoard - SetPlayer - " + score + ", start...");

            LeaderBoardPlayer currentPlayer = LeaderBoardPlayers.Find(player => player.Name == _currentPublicName);

            Debug.Log("YandexLeaderBoard - SetPlayer - currentPlayer: " + currentPlayer);

            if (currentPlayer != null && currentPlayer.Score >= score)
            {
                Debug.Log(
                    $"YandexLeaderBoard - SetPlayer - score less then at leaderboard: new score {score}, old score: {currentPlayer.Score} ");
                return;
            }

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

        public void GetCurrentPublicName()
        {
            if (PlayerAccount.IsAuthorized == false)
            {
                Debug.Log("YandexLeaderBoard - GetCurrentPublicName - IsAuthorized == false");
                return;
            }

            if (PlayerAccount.HasPersonalProfileDataPermission == false)
            {
                Debug.Log("YandexLeaderBoard - GetCurrentPublicName - HasPersonalProfileDataPermission == false");
                return;
            }

            PlayerAccount.GetProfileData(onSuccessCallback =>
                {
                    _currentPublicName = onSuccessCallback.publicName;
                    Debug.Log("YandexLeaderBoard - GetCurrentPublicName - SUCCESS, currentPublicName = " +
                              _currentPublicName);
                },
                onErrorCallback =>
                {
                    Debug.Log("YandexLeaderBoard - GetCurrentPublicName - ERROR: " + onErrorCallback);
                });
        }

        public async Task Fill()
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