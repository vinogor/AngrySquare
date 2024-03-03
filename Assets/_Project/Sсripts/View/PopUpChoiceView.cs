using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.View{
    public class PopUpChoiceView : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Image[] _images;

        public Button.ButtonClickedEvent[] ButtonsOnClick => _buttons.Select(button => button.onClick).ToArray();

        private void Awake()
        {
            Assert.AreEqual(3, _buttons.Length);
            Assert.AreEqual(3, _images.Length);
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