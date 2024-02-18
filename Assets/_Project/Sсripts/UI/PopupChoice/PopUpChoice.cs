using System.Linq;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI.PopupChoice
{
    public class PopUpChoice : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Image[] _images;

        public Button.ButtonClickedEvent[] ButtonsOnClick => _buttons.Select(button => button.onClick).ToArray();

        private void Awake()
        {
            // TODO: добавить везде где есть коллекции добавляемые через инспектор 
            Validator.ValidateAmount(_buttons, 3);
            Validator.ValidateAmount(_images, 3);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetSprites(Sprite[] sprites)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                _images[i].sprite = sprites[i];
            }
        }
    }
}