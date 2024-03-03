using TMPro;
using UnityEngine;

namespace _Project.View
{
    public class LeaderboardElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Initialize(string playerName, int playerScore)
        {
            _nameText.SetText(playerName);
            _scoreText.SetText(playerScore.ToString());
        }
    }
}