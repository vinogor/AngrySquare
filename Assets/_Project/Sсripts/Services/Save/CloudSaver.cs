using System;
using Agava.YandexGames;
using UnityEngine;

namespace _Project.Services.Save
{
    public class CloudSaver : ISaver
    {
        public void Write(string data)
        {
            Debug.Log("CloudSaver - save - STARTED");

            PlayerAccount.SetCloudSaveData(data,
                () => Debug.Log("CloudSaver - PlayerAccount.SetCloudSaveData - SUCCESS"),
                (message) => Debug.Log("CloudSaver - PlayerAccount.SetCloudSaveData - ERROR: " + message));
        }

        public void Read(Action<string> action)
        {
            Debug.Log("CloudSaver - load - STARTED");

            PlayerAccount.GetCloudSaveData((loadedData) =>
                {
                    Debug.Log("CloudSaver - PlayerAccount.GetCloudSaveData - COMPLETED");
                    action.Invoke(loadedData);
                },
                errorMessage =>
                {
                    Debug.Log($"CloudSaver - PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
                    action.Invoke(String.Empty);
                });
        }
    }
}