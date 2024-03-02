using System.Threading.Tasks;
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

        public Task<string> Read()
        {
            return Task.FromResult(PlayerPrefs.GetString(SaveKey));
        }
    }
}