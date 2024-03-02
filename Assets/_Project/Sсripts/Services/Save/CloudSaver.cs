using System.Threading.Tasks;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Sсripts.Services.Save
{
    public class CloudSaver : ISaver
    {
        public void Write(string data)
        {
            Debug.Log("CloudSave - STARTED");

            PlayerAccount.SetCloudSaveData(data,
                () => Debug.Log("PlayerAccount.SetCloudSaveData - SUCCESS"));
        }

        public async Task<string> Read()
        {
            Debug.Log("CloudLoad - STARTED");

            bool isCompleted = false;
            string data = string.Empty;

            PlayerAccount.GetCloudSaveData((loadedData) =>
                {
                    Debug.Log("PlayerAccount.GetCloudSaveData - COMPLETED");
                    data = loadedData;
                    isCompleted = true;
                },
                errorMessage =>
                {
                    Debug.Log($"PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
                    isCompleted = true;
                });

            await UniTask.WaitUntil(() => isCompleted);

            return await Task.FromResult(data);
        }
    }
}