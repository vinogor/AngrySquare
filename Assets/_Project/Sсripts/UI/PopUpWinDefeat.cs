using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI
{
    public class PopUpWinDefeat : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _info;

        private void Awake()
        {
            Assert.IsNotNull(_title);
            Assert.IsNotNull(_info);

            SetNotActive();
        }

        public void SetActive()
        {
            gameObject.SetActive(true);
        }

        public void SetNotActive()
        {
            gameObject.SetActive(false);
        }

        public void SetPlayerLoseInfo()
        {
            _title.text = "Player Lose";
            _info.text = "You lost the game!";
        }

        public void SetPlayerWinInfo()
        {
            _title.text = "Player Win";
            _info.text = "It's time to fight a new opponent!";
        }

        public void OnButtonClick()
        {
            SetNotActive();
            // TODO: переход в новый стейт ?
        }

        private void OnDestroy()
        {
            SetNotActive();
        }
    }
}