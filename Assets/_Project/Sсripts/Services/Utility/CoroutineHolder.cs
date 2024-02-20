using System.Collections;
using UnityEngine;

namespace _Project.Sсripts.Services.Utility
{
    public sealed class CoroutineHolder : MonoBehaviour
    {
        private static CoroutineHolder s_instance;

        public static CoroutineHolder Instance
        {
            get
            {
                if (s_instance == null)
                {
                    var gameObject = new GameObject("COROUTINE MANAGER");
                    s_instance = gameObject.AddComponent<CoroutineHolder>();
                    DontDestroyOnLoad(gameObject);
                }

                return s_instance;
            }
        }
    }

    public static class CoroutineExtension
    {
        public static Coroutine StartCoroutine(this IEnumerator coroutine)
            => CoroutineHolder.Instance.StartCoroutine(coroutine);

        public static Coroutine StartCoroutine(this IEnumerator coroutine, MonoBehaviour monoBehaviour)
            => monoBehaviour.StartCoroutine(coroutine);
    }
}