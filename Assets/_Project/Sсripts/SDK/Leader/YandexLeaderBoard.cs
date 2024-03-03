using System.Collections.Generic;
using Agava.YandexGames;

namespace _Project.SDK.Leader
{
    public class YandexLeaderBoard
    {
        private const string AnonymousName = "Anonymous";
        private const string LeaderBoardName = "LeaderBoard-AngrySquare";

        private readonly List<LeaderBoardPlayer> _leaderBoardPlayers = new();
        
        public void SetPlayer(int score)
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(LeaderBoardName,
                onSuccessCallback =>
                {
                    Leaderboard.SetScore(LeaderBoardName, score);
                });

            Fill();
        }

        public List<LeaderBoardPlayer> GetLeaderBoardPlayers()
        {
            return _leaderBoardPlayers;
        }

        private void Fill()
        {
            _leaderBoardPlayers.Clear();

            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetEntries(LeaderBoardName, onSuccessCallback: result =>
            {
                foreach (var entry in result.entries)
                {
                    var score = entry.score;
                    var name = entry.player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = AnonymousName;

                    _leaderBoardPlayers.Add(new LeaderBoardPlayer(score, name));
                }
            });
        }
    }
}