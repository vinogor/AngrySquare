using UnityEngine;

namespace Domain
{
    public class EnemyAim : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}