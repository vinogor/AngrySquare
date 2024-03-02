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

            bool isLoadCompleted = false;
            string data = string.Empty;

            PlayerAccount.GetCloudSaveData((loadedData) =>
                {
                    Debug.Log("CloudSaver - PlayerAccount.GetCloudSaveData - COMPLETED");
                    data = loadedData;
                    isLoadCompleted = true;
                },
                errorMessage =>
                {
                    Debug.Log($"CloudSaver - PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
                    isLoadCompleted = true;
                });

            Debug.Log("CloudSaver - waiting for 'isLoadCompleted' " + isLoadCompleted);
            
            
            await UniTask.WaitUntil(() => isLoadCompleted);

            return await Task.FromResult(data);
        }
    }
}