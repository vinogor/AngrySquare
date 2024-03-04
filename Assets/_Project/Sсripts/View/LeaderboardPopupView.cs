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
            ClearLeaderboard();
        }

        public void ConstructLeaderboard(List<LeaderBoardPlayer> leaderBoardPlayers)
        {
            for (int i = 0; i < leaderBoardPlayers.Count; i++)
            {
                if (_spawnedElements.Count <= i)
                {
                    LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
                    _spawnedElements.Add(leaderboardElementInstance);
                }

                _spawnedElements[i].Initialize(leaderBoardPlayers[i].Name, leaderBoardPlayers[i].Score);
            }

            for (int i = leaderBoardPlayers.Count; i < _spawnedElements.Count; i++)
                _spawnedElements[i].Hide();

            Show();
        }

        private void ClearLeaderboard()
        {
            foreach (LeaderboardElement element in _spawnedElements)
            {
                Destroy(element.gameObject);
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