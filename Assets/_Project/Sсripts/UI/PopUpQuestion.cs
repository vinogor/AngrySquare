using UnityEngine;

namespace _Project.Sсripts.UI
{
    public class PopUpQuestion : MonoBehaviour
    {
        
        private void Awake()
        {
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

        public void OnClickFirstButton()
        {
            
        }
        
        public void OnClickSecondButton()
        {
            
        }
        
        public void OnClickThirdButton()
        {
            
        }
        
    }
}