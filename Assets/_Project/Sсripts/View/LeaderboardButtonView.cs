using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class LeaderboardButtonView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;

        public Button.ButtonClickedEvent ButtonOnClick => _button.onClick;
    }
}