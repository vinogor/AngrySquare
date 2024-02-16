using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class PopUpQuestionController
    {
        
        
        
        private PopUpQuestion _popUpQuestion;
        
        
    }

    public class PopUpQuestion : MonoBehaviour
    {
        [SerializeField] [Required] private Button _button1;
        [SerializeField] [Required] private Button _button2;
        [SerializeField] [Required] private Button _button3;
        private void Awake()
        {
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
    }
}