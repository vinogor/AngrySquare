using System.Collections.Generic;
using _Project.SDK;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.View
{
    public class LeaderboardPopupView : MonoBehaviour
    {
        [SerializeField] private Button _buttonClose;
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _leaderboardElementPrefab;

        private List<LeaderboardElement> _spawnedElements = new();

        private void Awake()
        {
            Hide();
            _buttonClose.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            _buttonClose.onClick.RemoveListener(Hide);
        }

        public void ConstructLeaderboard(List<LeaderBoardPlayer> leaderBoardPlayers)
        {
            // TODO: не очищается почему-то! 
            ClearLeaderboard();

            foreach (LeaderBoardPlayer player in leaderBoardPlayers)
            {
                LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
                leaderboardElementInstance.Initialize(player.Name, player.Score);

                _spawnedElements.Add(leaderboardElementInstance);
            }

            Show();
        }

        private void ClearLeaderboard()
        {
            foreach (var element in _spawnedElements)
            {
                Destroy(element);
            }

            _spawnedElements = new List<LeaderboardElement>();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}