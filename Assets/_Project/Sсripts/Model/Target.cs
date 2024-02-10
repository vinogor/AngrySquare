using UnityEngine;

namespace Sсripts.Model
{
    public class Target : MonoBehaviour
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
    }
}