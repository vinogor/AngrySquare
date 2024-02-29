using System.Collections.Generic;
using Agava.YandexGames;

namespace _Project.Sсripts.SDK
{
    public class LeaderBoard
    {
        private const string AnonymousName = "Anonymous";
        private const string LeaderBoardName = "LeaderBoard-AngrySquare";

        // нигде не вызывается 
        private readonly List<LeaderBoardPlayer> _leaderBoardPlayers = new();

        // TODO: вызвать когда стейт Игрок выйграл  
        public void SetPlayer(int score)
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(LeaderBoardName,
                onSuccessCallback => { Leaderboard.SetScore(LeaderBoardName, score); });

            Fill();
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