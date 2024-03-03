using System;
using UnityEngine;

namespace _Project.Sсripts.Services.Save
{
    public class LocalSaver : ISaver
    {
        private const string SaveKey = "Save";

        public void Write(string data)
        {
            PlayerPrefs.SetString(SaveKey, data);
            PlayerPrefs.Save();
        }

        public void Read(Action<string> action)
        {
            // TODO: потом вернуть 
            // action.Invoke(PlayerPrefs.GetString(SaveKey));
            action.Invoke(PlayerPrefs.GetString("empty"));
        }
    }
}