using System.Collections;
using UnityEngine;

namespace _Project.Sсripts.Utility
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines s_instance;

        private static Coroutines Instance
        {
            get
            {
                if (s_instance == null)
                {
                    var gameObject = new GameObject("COROUTINE MANAGER");
                    s_instance = gameObject.AddComponent<Coroutines>();
                    DontDestroyOnLoad(gameObject);
                }

                return s_instance;
            }
        }

        public static Coroutine StartRoutine(IEnumerator coroutine)
        {
            return Instance.StartCoroutine(coroutine);
        }

        public static void StopRoutine(Coroutine coroutine)
        {
            Instance.StopCoroutine(coroutine);
        }
    }
}