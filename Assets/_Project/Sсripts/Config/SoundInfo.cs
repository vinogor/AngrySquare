using System;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine;

namespace _Project.Sсripts.Config
{
    [Serializable]
    public class SoundInfo
    {
        [field: SerializeField] public SoundName SoundName { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}