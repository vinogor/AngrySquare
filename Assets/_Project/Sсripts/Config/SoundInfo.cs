using System;
using Controllers.Sound;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class SoundInfo
    {
        [field: SerializeField] public SoundName SoundName { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}