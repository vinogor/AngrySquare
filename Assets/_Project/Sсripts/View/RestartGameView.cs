using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RestartGameView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public Button.ButtonClickedEvent ButtonClickedEvent => _button.onClick;
    }
}