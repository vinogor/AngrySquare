using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.View
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button;
        [SerializeField] [Required] private Image _image;
        [SerializeField] [Required] private Sprite _spriteOn;
        [SerializeField] [Required] private Sprite _spriteOff;

        public Button.ButtonClickedEvent ButtonOnClick => _button.onClick;

        private void Awake()
        {
            SetOn();
        }

        public void SetOn()
        {
            _image.sprite = _spriteOn;
        }

        public void SetOff()
        {
            _image.sprite = _spriteOff;
        }
    }
}