using System;
using Agava.YandexGames;
using UnityEngine;

namespace _Project.Sсripts.Services.Save
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

            string data = string.Empty;

            PlayerAccount.GetCloudSaveData((loadedData) =>
                {
                    Debug.Log("CloudSaver - PlayerAccount.GetCloudSaveData - COMPLETED");
                    data = loadedData;
                    action.Invoke(data);
                },
                errorMessage =>
                {
                    Debug.Log($"CloudSaver - PlayerAccount.GetCloudSaveData - ERROR: {errorMessage}");
                    action.Invoke(data);
                });
        }
    }
}