using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sсripts
{
    public class SdkInitialization : MonoBehaviour
    {
        // TODO: добавить какой-то экран загрузки

        private void Awake() => YandexGamesSdk.CallbackLogging = true;

        // TODO: ??? а если не в корутине, то что будет? 
        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(OnInitialized);
        }

        private void OnInitialized()
        {
            Debug.Log("YandexGamesSdk initialized");
            SceneManager.LoadScene("GameScene");
        }
    }
}