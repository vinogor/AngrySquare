using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Sсripts.SDK
{
    public class SdkInitialization : MonoBehaviour
    {
        // TODO: добавить какой-то экран загрузки?

        private void Awake() => YandexGamesSdk.CallbackLogging = true;

        private void Start()
        {
            YandexGamesSdk.Initialize(OnInitialized);
        }

        private void OnInitialized()
        {
            Debug.Log("YandexGamesSdk initialized");
            SceneManager.LoadScene("GameScene");
        }
    }
}