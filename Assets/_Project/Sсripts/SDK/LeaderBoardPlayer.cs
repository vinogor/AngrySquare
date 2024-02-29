namespace _Project.Sсripts.SDK
{
    public class LeaderBoardPlayer
    {
        public LeaderBoardPlayer(int score, string name)
        {
            Score = score;
            Name = name;
        }

        public int Score { get; private set; }
        public string Name { get; private set; }
    }
}