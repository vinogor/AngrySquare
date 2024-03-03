using System;
using UnityEngine;

namespace _Project.Services.Save
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
            action.Invoke(PlayerPrefs.GetString(SaveKey));
        }
    }
}