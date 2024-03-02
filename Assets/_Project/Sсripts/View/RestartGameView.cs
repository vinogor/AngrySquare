using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.View
{
    public class RestartGameView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public Button.ButtonClickedEvent ButtonClickedEvent => _button.onClick;
    }
}