using System;
using _Project.Controllers.Sound;
using UnityEngine;

namespace _Project.Config
{
    [Serializable]
    public class SoundInfo
    {
        [field: SerializeField] public SoundName SoundName { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}