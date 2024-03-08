using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace View{
    public class PopUpChoiceView : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Image[] _images;
        
        private const int ExpectedTotalAmountItems = 3;

        public Button.ButtonClickedEvent[] ButtonsOnClick => _buttons.Select(button => button.onClick).ToArray();

        private void Awake()
        {
            Assert.AreEqual(ExpectedTotalAmountItems, _buttons.Length);
            Assert.AreEqual(ExpectedTotalAmountItems, _images.Length);
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